using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace sokoball_edit
{
    public partial class Form1 : Form
    {
        class TileItem
        {
            public Bitmap img;
            public Keys key;
        }

        const string DIR_IMG = "img";
        const int IMG_SIZE = 25;
        const int TILES_X = 19;
        const int TILES_Y = 17;
        const int EDIT_CTRL_MARGIN = 12;
        const Keys TILES_BLANK = Keys.F;
        //
        Control edit_area;
        List<TileItem> tile_pics = new List<TileItem>();
        TileItem[,] tile_data = new TileItem[TILES_X, TILES_Y];
        Graphics gfx;
        Pen edit_pen = new Pen(Color.FromArgb(0, 255, 0), 1);
        int cur_x, cur_y;
        //
        Dictionary<short, Keys> tile_convert = new Dictionary<short, Keys>()
        {
            {0x0A,Keys.B},
            {0x03,Keys.F},
            {0x04,Keys.T},
            {0x06,Keys.W},
            {0x12,Keys.E},
            {0x09,Keys.H},
            {0x11,Keys.Z},
            {0x13,Keys.S},
            //
            {0x07,Keys.Q},
            {0x08,Keys.M},
            {0x10,Keys.N},
            {0x14,Keys.O},
            {0x15,Keys.I},
            {0x0B,Keys.L},
            {0x0C,Keys.R},
            {0x0D,Keys.U},
            {0x0E,Keys.D},
            {0x0F,Keys.C},
        };


        public Form1()
        {
            int edit_w = IMG_SIZE * TILES_X;
            int edit_h = IMG_SIZE * TILES_Y;
            InitializeComponent();
            SuspendLayout();
            try
            {
                ClientSize = new Size(edit_w + 2 * EDIT_CTRL_MARGIN + 120, edit_h + 2 * EDIT_CTRL_MARGIN);
                edit_area = new Control();
                edit_area.SetBounds(EDIT_CTRL_MARGIN, EDIT_CTRL_MARGIN, edit_w, edit_h);
                Bitmap tmp_img = new Bitmap(edit_w, edit_h);
                edit_area.BackgroundImage = tmp_img;
                gfx = Graphics.FromImage(tmp_img);
                Controls.Add(edit_area);
                foreach (string fname in Directory.GetFiles(DIR_IMG, "*.png"))
                {
                    TileItem itm = new TileItem();
                    itm.img = new Bitmap(fname);
                    itm.key = (Keys)Enum.Parse(typeof(Keys), Path.GetFileNameWithoutExtension(fname));
                    tile_pics.Add(itm);
                }
                cur_x = 0;
                cur_y = 0;
                TileItem blank_tile = tile_pics.SingleOrDefault(x => x.key == TILES_BLANK);
                for (int i = 0; i < TILES_X; i++)
                {
                    for (int j = 0; j < TILES_Y; j++)
                    {
                        tile_data[i, j] = blank_tile;
                        gfx.DrawImage(blank_tile.img, i * IMG_SIZE, j * IMG_SIZE, IMG_SIZE, IMG_SIZE);
                    }
                }
                lb_legend.BackgroundImage = new Bitmap("editor.png");
                lb_legend.Size = lb_legend.BackgroundImage.Size;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Init failed\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            ResumeLayout();
            //LoadData("BANZAI.SOK");
            //SaveData("TEST.SOK");
            MoveCursor(() => { });
        }

        private void LoadData(string fname)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(fname, FileMode.Open, FileAccess.Read)))
                {
                    br.ReadBytes(7);
                    int cnt = 0;
                    while (br.PeekChar() > -1 && cnt < TILES_X * TILES_Y)
                    {
                        Keys tmp;
                        tile_convert.TryGetValue(br.ReadInt16(), out tmp);
                        TileItem itm = tile_pics.SingleOrDefault(x => x.key == tmp) ?? tile_pics[0];
                        tile_data[cnt % TILES_X, cnt / TILES_X] = itm;
                        cnt++;
                        if (cnt % TILES_X == 0)
                        {
                            br.ReadInt32();
                        }
                    }
                }
                for (int i = 0; i < TILES_X; i++)
                {
                    for (int j = 0; j < TILES_Y; j++)
                    {
                        gfx.DrawImage(tile_data[i, j].img, i * IMG_SIZE, j * IMG_SIZE, IMG_SIZE, IMG_SIZE);
                    }
                }
                edit_area.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load failed:\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveData(string fname)
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(fname, File.Exists(fname) ? FileMode.Truncate : FileMode.CreateNew, FileAccess.Write)))
                {
                    bw.Write(new byte[] { 0xFD, 0x0F, 0x2D, 0xA6, 0x00, 0xF8, 0x02 });
                    for (int j = 0; j < TILES_Y; j++)
                    {
                        for (int i = 0; i < TILES_X; i++)
                        {
                            KeyValuePair<short, Keys> itm = tile_convert.FirstOrDefault(x => x.Value == tile_data[i, j].key);
                            bw.Write(itm.Key);
                        }
                        bw.Write(0);
                    }
                    for (int i = 0; i < 0x2E; i++) bw.Write((byte)0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed:\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MoveCursor(Action hndl)
        {
            int x = cur_x * IMG_SIZE;
            int y = cur_y * IMG_SIZE;
            gfx.DrawImage(tile_data[cur_x, cur_y].img, x, y, IMG_SIZE, IMG_SIZE);
            hndl();
            x = cur_x * IMG_SIZE;
            y = cur_y * IMG_SIZE;
            gfx.DrawImage(tile_data[cur_x, cur_y].img, x, y, IMG_SIZE, IMG_SIZE);
            gfx.DrawRectangle(edit_pen, x + 1, y + 1, IMG_SIZE - 2, IMG_SIZE - 2);
            edit_area.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                MoveCursor(() => { if (cur_y > 0) cur_y--; });
                goto end;
            }
            else if (keyData == Keys.Down)
            {
                MoveCursor(() => { if (cur_y < TILES_Y - 1) cur_y++; });
                goto end;
            }
            else if (keyData == Keys.Left)
            {
                MoveCursor(() => { if (cur_x > 0) cur_x--; });
                goto end;
            }
            else if (keyData == Keys.Right)
            {
                MoveCursor(() => { if (cur_x < TILES_X - 1) cur_x++; });
                goto end;
            }
            TileItem itm = tile_pics.SingleOrDefault(x => x.key == keyData);
            if (itm == null) goto end;
            tile_data[cur_x, cur_y] = itm;
            gfx.DrawImage(itm.img, cur_x * IMG_SIZE, cur_y * IMG_SIZE, IMG_SIZE, IMG_SIZE);
            edit_area.Invalidate();
        end:
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void mi_load_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() != DialogResult.OK) return;
            LoadData(ofd.FileName);
        }

        private void mi_save_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() != DialogResult.OK) return;
            SaveData(sfd.FileName);
        }
    }
}
