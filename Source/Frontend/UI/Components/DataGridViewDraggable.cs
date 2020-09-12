namespace RTCV.UI.Components
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    //https://stackoverflow.com/a/2372976
    /// <summary>
    /// Data Grid view with a custom drag drop implementation
    /// </summary>
    public partial class DataGridViewDraggable : DataGridView
    {
        private bool _delayedMouseDown = false;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

        public DataGridViewDraggable()
        {
            base.DragDrop += DragDrop;
            base.DragOver += DragOver;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            base.OnCellMouseDown(e);

            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right && this.CurrentRow != null)
            {
                var currentRow = this.CurrentRow.Index;
                var selectedRows = this.SelectedRows.OfType<DataGridViewRow>().ToList();
                var clickedRowSelected = this.Rows[e.RowIndex].Selected;

                this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Select previously selected rows, if control is down or the clicked row was already selected
                if ((ModifierKeys & Keys.Control) != 0 || clickedRowSelected)
                {
                    selectedRows.ForEach(row => row.Selected = true);
                }

                // Select a range of new rows, if shift key is down
                if ((ModifierKeys & Keys.Shift) != 0)
                {
                    for (var i = currentRow; i != e.RowIndex; i += Math.Sign(e.RowIndex - currentRow))
                    {
                        this.Rows[i].Selected = true;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var rowIndex = base.HitTest(e.X, e.Y).RowIndex;
            _delayedMouseDown = (rowIndex >= 0 &&
                (ModifierKeys & Keys.Alt) > 0);

            if (!_delayedMouseDown)
            {
                base.OnMouseDown(e);

                if (rowIndex >= 0)
                {
                    // Remember the point where the mouse down occurred.
                    // The DragSize indicates the size that the mouse can move
                    // before a drag event should be started.
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    _dragBoxFromMouseDown = new Rectangle(
                        new Point(e.X - (dragSize.Width / 4), e.Y - (dragSize.Height / 4)), dragSize);
                }
                else
                {
                    // Reset the rectangle if the mouse is not over an item in the datagridview.
                    _dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Perform the delayed mouse down before the mouse up
            if (_delayedMouseDown)
            {
                _delayedMouseDown = false;
                base.OnMouseDown(e);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            base.OnMouseMove(e);

            // If the mouse moves outside the rectangle, start the drag.
            if (this.SelectedRows != null && (e.Button & MouseButtons.Left) > 0 &&
                _dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                if (_delayedMouseDown)
                {
                    _delayedMouseDown = false;
                    if ((ModifierKeys & Keys.Control) > 0)
                    {
                        base.OnMouseDown(e);
                    }
                }

                // Proceed with the drag and drop, passing in the drag data
                var dragData = this.SelectedRows;
                this.DoDragDrop(dragData, DragDropEffects.Move);
            }
        }

        private int rowIndexOfItemUnderMouseToDrop;

        private new void DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be
            // converted to client coordinates.
            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                var rows = e.Data.GetData(typeof(DataGridViewSelectedRowCollection)) as DataGridViewSelectedRowCollection;

                if (rows != null)
                {
                    //We want to keep things in their original order rather than the order in which they were selected. Therefore sort by their rowindex as a key
                    DataGridViewRow[] _rows = rows.Cast<DataGridViewRow>().ToArray();
                    _rows = _rows.OrderBy(it => it.Index).ToArray();

                    //Go in reverse since the collection seems to be backwards?
                    foreach (DataGridViewRow row in _rows)
                    {
                        this.Rows.Remove(row);
                    }

                    // Get the row index of the item the mouse is below.
                    var hitTest = this.HitTest(clientPoint.X, clientPoint.Y);
                    rowIndexOfItemUnderMouseToDrop = hitTest.RowIndex;
                    if (rowIndexOfItemUnderMouseToDrop == -1)
                    {
                        //Do a global hittest to figure out if we're on the header since you get -1 on both the header and the area below
                        if (clientPoint.Y <= this.ColumnHeadersHeight)
                        {
                            rowIndexOfItemUnderMouseToDrop = 0;
                        }
                        else
                        {
                            rowIndexOfItemUnderMouseToDrop = Math.Max(this.Rows.Count, 0);
                        }
                    }

                    //We InsertRange rather than inserting in the iterator so we don't have to deal with the edge case of moving two items up by one position goofing the indexes
                    this.Rows.InsertRange(rowIndexOfItemUnderMouseToDrop, _rows);

                    //Re-select the new rows
                    this.ClearSelection();
                    for (var i = rowIndexOfItemUnderMouseToDrop; i < (rowIndexOfItemUnderMouseToDrop + _rows.Length); i++)
                    {
                        this.Rows[i].Selected = true;
                    }
                }
            }
        }

        private new void DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            var headeroffset = this.Top + this.ColumnHeadersHeight;

            Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

            if (clientPoint.Y < headeroffset && FirstDisplayedScrollingRowIndex > 0)
            {
                this.FirstDisplayedScrollingRowIndex -= 1;
            }
            else if (clientPoint.Y > this.Bottom - 60)
            {
                this.FirstDisplayedScrollingRowIndex += 1;
            }
        }
    }
}
