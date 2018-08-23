using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public class MultiGrid
    {
        // multiple grids that will load on multiple forms

        public CanvasGrid[] grids;

        public MultiGrid(params CanvasGrid[] _grids)
        {
            grids = _grids;
        }

        public void Load()
        {
            //Deploys and loads all the grids in the multigrid object

            for(int i=0;i<grids.Count();i++)
            {
                if (i == 0)
                    UI_CanvasForm.loadTileFormMain(grids[i]);
                else
                    UI_CanvasForm.loadTileFormExtra(grids[i]);

            }
        }
    }

    public class CanvasGrid
    {
        //grid that represents the layout of a single form

        public int x = 0;
        public int y = 0;
        public string[,] grid;

        public CanvasGrid(int _x, int _y)
        {
            x = _x;
            y = _y;
            grid = new string[x,y];
        }

        public void SetTileForm(string tileForm, int tilePosX, int tilePosY)
        {
            //removes tileForm position if already exists
            if(!tileForm.Contains("DummyTileForm"))
                for (int _x = 0; _x < x; _x++)
                    for (int _y = 0; _y < y; _y++)
                        if (grid[_x, _y] == tileForm)
                            grid[_x, _y] = null;

            //place tileForm if within grid space
            if (tilePosX < x && tilePosY < y)
                grid[tilePosX, tilePosY] = tileForm;

        }

    }

}
