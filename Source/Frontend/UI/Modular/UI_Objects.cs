using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTCV.UI
{

    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool HasCancelButton { get; set; }
        void SubForm_Cancel();
        void SubForm_Ok();
    }


    public interface ITileForm
    {
        //Interface used for added contrals in TileForms

        int TilesX { get; set; }
        int TilesY { get; set; }
        bool CanPopout { get; set; }

    }

    public class CanvasGrid
    {
        //grid that represents the layout of a single form

        public int x = 0;
        public int y = 0;
        public Form[,] gridComponent;
        public Size?[,] gridComponentSize;
        public bool?[,] gridComponentDisplayHeader;

        public string GridName = "";
        internal bool isResizable = false;

        public CanvasGrid(int _x, int _y, string _GridName)
        {
            x = _x;
            y = _y;
            gridComponent = new Form[x,y];
            gridComponentSize = new Size?[x, y];
            gridComponentDisplayHeader = new bool?[x, y];
            GridName = _GridName;
        }

        public void SetTileForm(Form componentForm, int tilePosX, int tilePosY, int tileSizeX, int tileSizeY, bool displayHeader, AnchorStyles anchor = (AnchorStyles.Top | AnchorStyles.Left))
        {
            //removes tileForm position if already exists
            for (int _x = 0; _x < x; _x++)
                for (int _y = 0; _y < y; _y++)
                    if (gridComponent[_x, _y] == componentForm)
                    {
                        gridComponent[_x, _y] = null;
                        gridComponentSize[_x, _y] = null;
                        gridComponentDisplayHeader[_x, _y] = null;
                    }

            //place tileForm if within grid space
            if (tilePosX < x && tilePosY < y)
            {
                gridComponent[tilePosX, tilePosY] = componentForm;
                gridComponentSize[tilePosX, tilePosY] = new Size(tileSizeX, tileSizeY);
                gridComponentDisplayHeader[tilePosX, tilePosY] = displayHeader;
            }

            componentForm.Anchor = anchor;
        }

        internal void LoadToMain()
        {
            UI_CanvasForm.loadTileFormMain(this);
        }

        internal void LoadToNewWindow(string GridID = null, bool silent = false)
        {
            UI_CanvasForm.loadTileFormExtraWindow(this, GridID, silent);
        }
    }

}
