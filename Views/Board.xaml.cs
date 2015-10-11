using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Wall.UserControls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Wall.Views
{
    public sealed partial class Board : UserControl
    {
        #region Değişkenler, asbitler, sınıf üyeleri

        // Satır-sütun sayısı
        private const int RC_COUNT = 9;

        // Hücreler
        private Cell[] Cells = new Cell[RC_COUNT * RC_COUNT - (RC_COUNT - 1) / 2];

        #endregion

        #region Kurucu metod

        /// <summary>
        /// Kurucu metod - Constructor
        /// </summary>
        public Board()
        {
            this.InitializeComponent();

            this.GameGrid.Width = 360;
            this.GameGrid.Height = 360 - RC_COUNT * 4;
            this.CreateGameBoard();
        }

        #endregion

        #region GameBoard'ın oluşturulması ve Cell event metodları

        /// <summary>
        /// Oyun tahtasını oluşturur
        /// </summary>
        private void CreateGameBoard()
        {
            this.CreateCells();

            int itemNo = 0;

            for (int row = 0; row < RC_COUNT; row++)
            {
                this.GameGrid.RowDefinitions.Add(new RowDefinition());
                Grid grid = new Grid();
                if (row % 2 == 0)
                {
                    for (int column = 0; column < RC_COUNT; column++)
                    {
                        grid.Margin = new Thickness(0, -2, 0, -2);
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        Grid.SetColumn(this.Cells[itemNo], column);
                        grid.Children.Add(this.Cells[itemNo]);
                        this.Cells[itemNo].Name = "D" + row + "" + column;
                        this.Cells[itemNo].Coordinate = new Point(row, column);
                        itemNo++;
                    }
                }
                else
                {
                    for (int column = 0; column < RC_COUNT - 1; column++)
                    {
                        double d = (this.GameGrid.Width / RC_COUNT) / 2;
                        grid.Margin = new Thickness(d, -2, d, -2);
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        Grid.SetColumn(this.Cells[itemNo], column);
                        grid.Children.Add(this.Cells[itemNo]);
                        this.Cells[itemNo].Name = "D" + row + "" + column;
                        this.Cells[itemNo].Coordinate = new Point(row, column);
                        itemNo++;
                    }
                }

                Grid.SetRow(grid, row);
                this.GameGrid.Children.Add(grid);
            }
        }

        /// <summary>
        /// Cell elemanlarını oluşturur
        /// </summary>
        private void CreateCells()
        {
            int itemcount = RC_COUNT * RC_COUNT - (RC_COUNT - 1) / 2;

            for (int i = 0; i < itemcount; i++)
            {
                this.Cells[i] = new Cell();
                this.Cells[i].Tapped += Cell_Tapped;
            }
        }

        /// <summary>
        /// Elemanların click handler metodu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Cell_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as Cell).IsSelected = !(sender as Cell).IsSelected;
        }

        #endregion

        #region Yardımcı metodlar

        /// <summary>
        /// Verilen koordinatın çevre elemanlarının listesini verir
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Cell> GetAroundItems(Cell cell)
        {
            double x = cell.Coordinate.X;
            double y = cell.Coordinate.Y;

            if (x < 0 || x >= RC_COUNT || y < 0 || y >= RC_COUNT)
                throw new Exception("Sınır dışı indis! (GetAroundItems() metodu)");

            Cell left = this.FindName("D" + x + "" + (y - 1)) as Cell;
            Cell left_top = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;
            Cell left_bottom = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;
            Cell right = this.FindName("D" + x + "" + (y + 1)) as Cell;
            Cell right_top = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;
            Cell right_bottom = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;

            // En yakın kenara göre yakın elemanlar listesi sırası ayarlanır
            List<Cell> around = new List<Cell>();
            switch (this.NearlyEdge(cell.Coordinate))
            {
                case Edge.Left:
                    {
                        if (left != null)
                            around.Add(left);
                        if (left_top != null)
                            around.Add(left_top);
                        if (left_bottom != null)
                            around.Add(left_bottom);
                        if (right_top != null)
                            around.Add(right_top);
                        if (right_bottom != null)
                            around.Add(right_bottom);
                        if (right != null)
                            around.Add(right);
                    }
                    break;
                case Edge.Top: ;
                    {
                        if (left_top != null)
                            around.Add(left_top);
                        if (right_top != null)
                            around.Add(right_top);
                        if (left != null)
                            around.Add(left);
                        if (right != null)
                            around.Add(right);
                        if (left_bottom != null)
                            around.Add(left_bottom);
                        if (right_bottom != null)
                            around.Add(right_bottom);
                    }
                    break;
                case Edge.Right: ;
                    {
                        if (right != null)
                            around.Add(right);
                        if (right_top != null)
                            around.Add(right_top);
                        if (right_bottom != null)
                            around.Add(right_bottom);
                        if (left_top != null)
                            around.Add(left_top);
                        if (left_bottom != null)
                            around.Add(left_bottom);
                        if (left != null)
                            around.Add(left);
                    }
                    break;
                case Edge.Bottom: ;
                    {
                        if (left_bottom != null)
                            around.Add(left_bottom);
                        if (right_bottom != null)
                            around.Add(right_bottom);
                        if (left != null)
                            around.Add(left);
                        if (right != null)
                            around.Add(right);
                        if (left_top != null)
                            around.Add(left_top);
                        if (right_top != null)
                            around.Add(right_top);
                    }
                    break;
            }

            return around;
        }


        /// <summary>
        /// Verilen koordinatın solundaki elemanlarının listesini verir
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Cell> GetLeftItems(Point c)
        {
            double x = c.X;
            double y = c.Y;

            if (x < 0 || x >= RC_COUNT || y < 0 || y >= RC_COUNT)
                throw new Exception("Sınır dışı indis! (GetAroundItems() metodu)");

            List<Cell> circleItems = new List<Cell>();
            Cell item1 = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;
            Cell item3 = this.FindName("D" + x + "" + (y - 1)) as Cell;
            Cell item5 = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;

            if (item3 != null)
                circleItems.Add(item3);
            if (item1 != null)
                circleItems.Add(item1);
            if (item5 != null)
                circleItems.Add(item5);

            return circleItems;
        }

        /// <summary>
        /// Verilen koordinatın solundaki elemanlarının listesini verir
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Cell> GetRightItems(Point c)
        {
            double x = c.X;
            double y = c.Y;

            if (x < 0 || x >= RC_COUNT || y < 0 || y >= RC_COUNT)
                throw new Exception("Sınır dışı indis! (GetAroundItems() metodu)");

            List<Cell> circleItems = new List<Cell>();

            Cell item2 = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;
            Cell item4 = this.FindName("D" + x + "" + (y + 1)) as Cell;
            Cell item6 = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;

            if (item4 != null)
                circleItems.Add(item4);
            if (item2 != null)
                circleItems.Add(item2);
            if (item6 != null)
                circleItems.Add(item6);

            return circleItems;
        }

        /// <summary>
        /// Verilen koordinatın üstündeki elemanlarının listesini verir
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Cell> GetTopItems(Point c)
        {
            double x = c.X;
            double y = c.Y;

            if (x < 0 || x >= RC_COUNT || y < 0 || y >= RC_COUNT)
                throw new Exception("Sınır dışı indis! (GetAroundItems() metodu)");

            List<Cell> circleItems = new List<Cell>();
            Cell item1 = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;
            Cell item2 = this.FindName("D" + (x - 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;

            if (item1 != null)
                circleItems.Add(item1);
            if (item2 != null)
                circleItems.Add(item2);

            return circleItems;
        }

        /// <summary>
        /// Verilen koordinatın altındaki elemanlarının listesini verir
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Cell> GetBottomItems(Point c)
        {
            double x = c.X;
            double y = c.Y;

            if (x < 0 || x >= RC_COUNT || y < 0 || y >= RC_COUNT)
                throw new Exception("Sınır dışı indis! (GetAroundItems() metodu)");

            List<Cell> circleItems = new List<Cell>();

            Cell item5 = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y - 1 : y)) as Cell;
            Cell item6 = this.FindName("D" + (x + 1) + "" + (x % 2 == 0 ? y : y + 1)) as Cell;

            if (item5 != null)
                circleItems.Add(item5);
            if (item6 != null)
                circleItems.Add(item6);

            return circleItems;
        }

        /// <summary>
        /// En yakın kenarı verir
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Edge NearlyEdge(Point p)
        {
            int toLeft = (int)p.X;
            int toTop = (int)p.Y;
            int toRight = RC_COUNT - (int)p.X - 1;
            int toBottom = RC_COUNT - (int)p.Y - 1;

            if (toLeft <= toTop && toLeft <= toRight && toLeft <= toBottom)
                return Edge.Left;
            if (toTop <= toLeft && toTop <= toRight && toTop <= toBottom)
                return Edge.Top;
            if (toRight <= toTop && toRight <= toLeft && toRight <= toBottom)
                return Edge.Right;
            else
                return Edge.Bottom;
        }

        /// <summary>
        /// Eleman kenarda mı?
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool IsOnEdge(Cell cell)
        {
            return (cell.Coordinate.X == 0 || cell.Coordinate.X == RC_COUNT - 1 || cell.Coordinate.Y == 0 || cell.Coordinate.Y == RC_COUNT - 1);
        }

        #endregion
    }

    /// <summary>
    /// Kenarlar
    /// </summary>
    public enum Edge
    {
        Left, Top, Right, Bottom
    }
}
