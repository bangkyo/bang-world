using C1.Win.C1FlexGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLib
{
    public class clsFlexGrid
    {
        clsStyle clsStyle = new clsStyle();

        public readonly float CommonFontSize = 12f;
        public readonly float CaptionComFontSize = 12f;
        public readonly bool IsSubTotal = true;

        public void GridCellRangeStyleColor(C1FlexGrid grdItem, int Row, int Col, Color cBackColor, Color cForeColor)
        {
            this.GridCellRangeStyleColor(grdItem, Row, Col, Row, Col, cBackColor, cForeColor);
        }
        public void GridCellRangeStyleColor(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, Color cBackColor, Color cForeColor)
        {
            CellStyle cStyle = null;
            if (grdItem.Styles.Contains("TopRow" + Convert.ToString(Row1) + "LeftCol" + Convert.ToString(Col1) + "BottomRow" + Convert.ToString(Row2) + "RightCol" + Convert.ToString(Col2)) == true)
            {
                grdItem.Styles["TopRow" + Convert.ToString(Row1) + "LeftCol" + Convert.ToString(Col1) + "BottomRow" + Convert.ToString(Row2) + "RightCol" + Convert.ToString(Col2)].Clear(StyleElementFlags.BackColor);
                grdItem.Styles["TopRow" + Convert.ToString(Row1) + "LeftCol" + Convert.ToString(Col1) + "BottomRow" + Convert.ToString(Row2) + "RightCol" + Convert.ToString(Col2)].Clear(StyleElementFlags.ForeColor); 
            }

            cStyle = grdItem.Styles.Add("TopRow" + Convert.ToString(Row1) + "LeftCol" + Convert.ToString(Col1) + "BottomRow" + Convert.ToString(Row2) + "RightCol" + Convert.ToString(Col2));
            cStyle.BackColor = cBackColor;
            cStyle.ForeColor = cForeColor;

            //cStyle.TextAlign = TextAlignEnum.CenterCenter;

            CellRange cRange = grdItem.GetCellRange(Row1, Col1, Row2, Col2);
            cRange.Style = cStyle;
        }

        public void GridCellStyleColor(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, Color cBackColor, Color cForeColor)
        {
            CellStyle cStyle1;
            CellStyle cStyle2;
            for (int nRow = Row1; nRow <= Row2; nRow++)
            {
                for (int nCol = Col1; nCol <= Col2; nCol++)
                {
                    cStyle2 = grdItem.GetCellStyle(nRow, nCol);
                    if(cStyle2 != null)
                    {
                        string BaseOnName = cStyle2.Name;

                        if(grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.BackColor);
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.ForeColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol), BaseOnName);
                        cStyle1.BackColor = cBackColor;
                        cStyle1.ForeColor = cForeColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                    else
                    {
                        if (grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.BackColor);
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.ForeColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol));
                        cStyle1.BackColor = cBackColor;
                        cStyle1.ForeColor = cForeColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                }
            }
        }

        public void GridCellStyleForeGroundColor(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, Color cForeColor)
        {
            CellStyle cStyle1;
            CellStyle cStyle2;
            for (int nRow = Row1; nRow <= Row2; nRow++)
            {
                for (int nCol = Col1; nCol <= Col2; nCol++)
                {
                    cStyle2 = grdItem.GetCellStyle(nRow, nCol);
                    if (cStyle2 != null)
                    {
                        string BaseOnName = cStyle2.Name;

                        if (grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.ForeColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol), BaseOnName);
                        cStyle1.ForeColor = cForeColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                    else
                    {
                        if (grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.ForeColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol));
                        cStyle1.ForeColor = cForeColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                }
            }
        }


        public void GridCellStyleBackGroundColor(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, Color cBackColor)
        {
            CellStyle cStyle1;
            CellStyle cStyle2;
            for (int nRow=Row1; nRow <= Row2; nRow++)
            {
                for(int nCol=Col1; nCol <= Col2; nCol++)
                {
                    cStyle2 = grdItem.GetCellStyle(nRow, nCol);
                    if (cStyle2 != null)
                    {
                        string BaseOnName = cStyle2.Name;

                        if (grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.BackColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol), BaseOnName);
                        cStyle1.BackColor = cBackColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                    else
                    {
                        if (grdItem.Styles.Contains("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)) == true)
                        {
                            grdItem.Styles["Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol)].Clear(StyleElementFlags.BackColor);
                        }

                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol));
                        cStyle1.BackColor = cBackColor;

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                }
            }
        }

        public void GridCellRangeStyle(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, CellStyle pStyle)
        {
            CellStyle cStyle = grdItem.Styles.Add("TopRow" + Convert.ToString(Row1) + "LeftCol" + Convert.ToString(Col1) + "BottomRow" + Convert.ToString(Row2) + "RightCol" + Convert.ToString(Col2), pStyle);
            //cStyle = pStyle;

            CellRange cRange = grdItem.GetCellRange(Row1, Col1, Row2, Col2);
            cRange.Style = cStyle;
        }

        public void GridCellStyle(C1FlexGrid grdItem, int Row1, int Col1, int Row2, int Col2, CellStyle pStyle)
        {
            CellStyle cStyle1;
            for (int nRow = Row1; nRow <= Row2; nRow++)
            {
                for (int nCol = Col1; nCol <= Col2; nCol++)
                {
                    if (pStyle != null)
                    {
                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol), pStyle);
                        //cStyle1 = pStyle;
                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                    else
                    {
                        cStyle1 = grdItem.Styles.Add("Row" + Convert.ToString(nRow) + "Col" + Convert.ToString(nCol));

                        grdItem.SetCellStyle(nRow, nCol, cStyle1);
                    }
                }
            }
        }

        public void GridColumnStyle(C1FlexGrid grdItem, int Col1, int Col2, CellStyle pStyle, StyleElementFlags ElementFlags = StyleElementFlags.TextAlign)
        {
            int Col = 0;
            Col = Col1;

            while (Col <= Col2)
            {
                //CellStyle cStyle = null;
                if (grdItem.Styles.Contains("Col" + Convert.ToString(Col)) == true)
                {
                    grdItem.Styles["Col" + Convert.ToString(Col)].Clear(StyleElementFlags.TextAlign);
                }

                CellStyle cStyle = grdItem.Styles.Add("Col" + Convert.ToString(Col), pStyle);
                cStyle.TextAlign = pStyle.TextAlign;
                grdItem.Cols[Col++].Style = cStyle;
            }
        }

        public void GridColumnStyle(C1FlexGrid grdItem, int Col1, CellStyle pStyle, StyleElementFlags ElementFlags = StyleElementFlags.TextAlign)
        {
            int Col = 0;
            Col = Col1;
            if (grdItem.Styles.Contains("Col" + Convert.ToString(Col)) == true)
            {
                if (grdItem.Styles.Contains("Col" + Convert.ToString(Col)) == true)
                {
                    grdItem.Styles["Col" + Convert.ToString(Col)].Clear(StyleElementFlags.TextAlign);
                }
            }

            CellStyle cStyle = grdItem.Styles.Add("Col" + Convert.ToString(Col), pStyle);
            cStyle.TextAlign = pStyle.TextAlign;
            grdItem.Cols[Col].Style = cStyle;
        }

        public void TopBorderStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            CellStyle cStyle = grdItem.Styles.Add("TopBorderStyle");
            clsStyle clsStyle = new clsStyle();
            cStyle.BackColor = clsStyle.ColorByTopBorder();
            cStyle.Border.Style = BorderStyleEnum.None;
            cStyle.Border.Color = clsStyle.ColorByTopBorder();

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cStyle);
        }

        public void DataGridCellRangeRightStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellRangeRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void DataGridCellRangeLeftStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellRangeLeftStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.LeftCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.LeftCenter;

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void DataGridCellRangeCenterStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellRangeCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void CaptionCellRangeColumnStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("CaptionCellRangeColumnStyle");
            cDataGridStyle.BackColor = clsStyle.ColorByCaptionBackGround();
            cDataGridStyle.ForeColor = clsStyle.ColorByCaptionForeGround();

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CaptionComFontSize, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void CaptionCellRangeRowStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("CaptionCellRangeRowStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;

            GridCellRangeStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

//----------------------------------------------------------------------------------------------------------------------------------------------------------


        public void DataGridCellRightStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridCellStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void DataGridCellLeftStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellLeftStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.LeftCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.LeftCenter;

            GridCellStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void DataGridCellCenterStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCellCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;
            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridCellStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void CaptionCellColumnStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("CaptionCellColumnStyle");
            cDataGridStyle.BackColor = clsStyle.ColorByCaptionBackGround();
            cDataGridStyle.ForeColor = clsStyle.ColorByCaptionForeGround();

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CaptionComFontSize, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;

            GridCellStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

        public void CaptionCellRowStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRow, int LeftCol, int BottomRow, int RightCol)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("CaptionCellRowStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;

            GridCellStyle(grdItem, TopRow, LeftCol, BottomRow, RightCol, cDataGridStyle);
        }

//-----------------------------------------------------------------------------------------------------------------------------------------------



        public void DataGridLeftStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridLeftStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.LeftCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.LeftCenter;

            GridColumnStyle(grdItem, Col, cDataGridStyle);
        }

        public void DataGridCenterStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridColumnStyle(grdItem, Col, cDataGridStyle);
        }

        public void DataGridRightStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridColumnStyle(grdItem, Col, cDataGridStyle);
        }

        public void DataGridLeftStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridLeftStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.LeftCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.LeftCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }

        public void DataGridCenterStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }
        public void DataGridCenterBoldStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }

        public void DataGridCenterBoldStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridCenterStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridColumnStyle(grdItem, Col, cDataGridStyle);
        }

        public void DataGridFixedCenterStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridFixedCenterStyle");
            cDataGridStyle.BackColor = Color.WhiteSmoke;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.CenterCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.CenterCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }

        public void DataGridRightStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Regular);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }

        public void DataGridRightBoldStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col1, int Col2)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 10.0f, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridColumnStyle(grdItem, Col1, Col2, cDataGridStyle);
        }

        public void DataGridRightBoldStyle(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int Col)
        {
            clsStyle clsStyle = new clsStyle();
            CellStyle cDataGridStyle = grdItem.Styles.Add("DataGridRightStyle");
            cDataGridStyle.BackColor = Color.White;
            cDataGridStyle.ForeColor = Color.Black;

            cDataGridStyle.Border.Color = clsStyle.ColorByCaptionBorder();
            cDataGridStyle.Border.Direction = BorderDirEnum.Both;
            cDataGridStyle.Border.Width = 1;
            cDataGridStyle.Border.Style = BorderStyleEnum.Flat;
            cDataGridStyle.Font = new Font("돋움체", 9.0f, FontStyle.Bold);
            cDataGridStyle.TextAlign = TextAlignEnum.RightCenter;
            //cDataGridStyle.ImageAlign = ImageAlignEnum.RightCenter;

            GridColumnStyle(grdItem, Col, cDataGridStyle);
        }

        public void FlexGridMain(C1FlexGrid grdItem, int RowsCount, int ColsCount, int RowsFixed, int RowsFrozen, int ColsFixed, int ColsFrozen)
        {
            grdItem.Rows.Count = RowsCount;
            grdItem.Rows.Fixed = RowsFixed;
            grdItem.Rows.Frozen = RowsFrozen;
            grdItem.Cols.Count = ColsCount;
            grdItem.Cols.Fixed = ColsFixed;
            grdItem.Cols.Frozen = ColsFrozen;

            grdItem.AllowDragging = AllowDraggingEnum.None;
            grdItem.AllowEditing = false;
            grdItem.AllowFreezing = AllowFreezingEnum.None;
            grdItem.AllowMerging = AllowMergingEnum.RestrictCols;
            grdItem.AllowResizing = AllowResizingEnum.Columns;
            grdItem.AllowSorting = AllowSortingEnum.SingleColumn;
            grdItem.AllowAddNew = false;
            grdItem.AllowDelete = false;
            grdItem.AutoClipboard = true;
            grdItem.AutoResize = false;
            grdItem.AutoSearch = AutoSearchEnum.FromTop;
            grdItem.AutoSearchDelay = 1;
            grdItem.KeyActionEnter = KeyActionEnum.MoveDown;
            grdItem.KeyActionTab = KeyActionEnum.MoveAcross;
            grdItem.ShowCellLabels = false;
            grdItem.SubtotalPosition = SubtotalPositionEnum.AboveData;

            grdItem.ExtendLastCol = false;
            grdItem.FocusRect = FocusRectEnum.None;
            grdItem.HighLight = HighLightEnum.Always;
            grdItem.ScrollBars = ScrollBars.Horizontal;
            grdItem.SelectionMode = SelectionModeEnum.Cell;

            ////grdItem.ShowSort = false;
            grdItem.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter;

            grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            grdItem.ShowButtons = ShowButtonsEnum.WithFocus;
            grdItem.ShowCursor = false;
            grdItem.ShowErrors = false;
            grdItem.UseWaitCursor = false;
        }

        //메뉴관리, 기준관리용 화면으로 사용되는 화면에 대한 스타일
        public void FlexGridMainSystem(C1FlexGrid grdItem, int RowsCount, int ColsCount, int RowsFixed, int RowsFrozen, int ColsFixed, int ColsFrozen)
        {
            grdItem.Rows.Count = RowsCount;
            grdItem.Rows.Fixed = RowsFixed;
            grdItem.Rows.Frozen = RowsFrozen;
            grdItem.Cols.Count = ColsCount;
            grdItem.Cols.Fixed = ColsFixed;
            grdItem.Cols.Frozen = ColsFrozen;

            grdItem.BeginUpdate();
            grdItem.Redraw = false;

            grdItem.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            grdItem.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;

            grdItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange;
            grdItem.AllowEditing = true;
            //소트 하도록 변경 2017.07.18
            grdItem.AllowSorting = AllowSortingEnum.SingleColumn;
            //사이즈 변경 하도록 변경 2017.07.18
            grdItem.AllowResizing = AllowResizingEnum.Columns;

            grdItem.Styles["Highlight"].BackColor = SystemColors.Highlight;
            grdItem.Styles["EmptyArea"].BackColor = Color.FromArgb(247, 247, 247);

            grdItem.MouseClick += griItem_MouseClick;

            grdItem.BackColor = Color.WhiteSmoke;
            grdItem.Styles.Alternate.BackColor = Color.White;
            grdItem.ExtendLastCol = true;


            CellStyle MDCellStyle = grdItem.Styles.Add("ModifyStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            MDCellStyle.Border.Color = Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            MDCellStyle.BackColor = Color.FromArgb(255, 219, 233, 246);
            MDCellStyle.ForeColor = Color.Blue;

            //clsFlexGrid.CaptionColumnStyle(fg, 0, 0, 0, fg.Cols.Count - 1);

            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count-1);
            //var crCellRange = fg.GetCellRange(0, 0, 0, fg.Cols.Count - 1);
            //crCellRange.Style = fg.Styles["CellStyle"];

            // 그리드 스타일(색) 추가
            CellStyle DelRs = grdItem.Styles.Add("DelColor");
            DelRs.BackColor = Color.Red;
            DelRs.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);

            CellStyle UpRs = grdItem.Styles.Add("UpColor");
            UpRs.BackColor = Color.Green;
            UpRs.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);

            CellStyle InsRs = grdItem.Styles.Add("InsColor");
            InsRs.BackColor = Color.Yellow;
            InsRs.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);


            var csCellStyle = grdItem.Styles.Add("CellStyle");
            //csCellStyle.Border.Color = Color.FromArgb(255, 125, 19, 215);

            //border 외각선 색
            csCellStyle.Border.Color = Color.Blue;//Color.FromArgb(255, 81, 181, 255);

            //header 색 지정
            csCellStyle.BackColor = Color.Blue; //Color.FromArgb(255, 218, 233, 245);
            csCellStyle.ForeColor = Color.White; //Color.FromArgb(255, 218, 233, 245);

            csCellStyle.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);


            var crCellRange = grdItem.GetCellRange(0, 0, 0, grdItem.Cols.Count - 1);
            crCellRange.Style = grdItem.Styles["CellStyle"];

            // Set styles for subtotals.
            //subtotal
            CellStyle s = grdItem.Styles[CellStyleEnum.GrandTotal];

            s.Border.Color = grdItem.Styles["CellStyle"].Border.Color;
            s.ForeColor = grdItem.Styles["CellStyle"].ForeColor;
            s.BackColor = grdItem.Styles["CellStyle"].BackColor;
            s.Font = grdItem.Styles["CellStyle"].Font;

            //s = grdItem.Styles[CellStyleEnum.Subtotal0];
            //s.BackColor = Color.DarkRed;
            //s.ForeColor = Color.White;
            //s.Font = new Font("돋움체", CommonFontSize, FontStyle.Bold);

            CellStyle cssubtotal = grdItem.Styles[CellStyleEnum.Subtotal0];

            cssubtotal.Border.Color = grdItem.Styles["CellStyle"].Border.Color;
            cssubtotal.ForeColor = grdItem.Styles["CellStyle"].ForeColor;
            cssubtotal.BackColor = grdItem.Styles["CellStyle"].BackColor;
            cssubtotal.Font = grdItem.Styles["CellStyle"].Font;

            //grdItem.Styles[CellStyleEnum.GrandTotal].BackColor = Color.Blue;
            //grdItem.Styles[CellStyleEnum.GrandTotal].ForeColor = Color.White;
            //CellStyle s = grdItem.Styles[CellStyleEnum.Subtotal0];

            //s.Border.Color = grdItem.Styles["Highlight"].Border.Color;
            //s.ForeColor = grdItem.Styles["Highlight"].ForeColor;
            //s.BackColor = grdItem.Styles["Highlight"].BackColor;

            //grdItem.AllowDragging = AllowDraggingEnum.None;
            //grdItem.AllowEditing = false;
            //grdItem.AllowFreezing = AllowFreezingEnum.None;
            //grdItem.AllowMerging = AllowMergingEnum.RestrictCols;
            //grdItem.AllowResizing = AllowResizingEnum.None;
            //grdItem.AllowSorting = AllowSortingEnum.None;
            //grdItem.AllowAddNew = false;
            //grdItem.AllowDelete = false;
            //grdItem.AutoClipboard = true;
            //grdItem.AutoResize = false;
            //grdItem.AutoSearch = AutoSearchEnum.FromTop;
            //grdItem.AutoSearchDelay = 1;
            //grdItem.KeyActionEnter = KeyActionEnum.MoveDown;
            //grdItem.KeyActionTab = KeyActionEnum.MoveAcross;
            //grdItem.ShowCellLabels = false;
            //grdItem.SubtotalPosition = SubtotalPositionEnum.AboveData;

            //grdItem.ExtendLastCol = false;
            //grdItem.FocusRect = FocusRectEnum.None;
            //grdItem.HighLight = HighLightEnum.Always;
            //grdItem.ScrollBars = ScrollBars.Horizontal;
            //grdItem.SelectionMode = SelectionModeEnum.Cell;
            //////grdItem.ShowSort = false;
            //grdItem.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter;

            //grdItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle;
            //grdItem.ShowButtons = ShowButtonsEnum.WithFocus;
            //grdItem.ShowCursor = false;
            //grdItem.ShowErrors = false;
            //grdItem.UseWaitCursor = false;

            grdItem.Redraw = true;
            grdItem.EndUpdate();
        }

        public  void DataGridFormat(C1FlexGrid _grdItem, int Col1, int Col2, string _format)
        {

            for (int col = Col1; col <=Col2; col++)
            {
                _grdItem.Cols[col].Format = _format;
            }

        }
        private void griItem_MouseClick(object sender, MouseEventArgs e)
        {
            C1FlexGrid grd = sender as C1FlexGrid;

            if (Control.ModifierKeys == Keys.Control)
            {
                int row = grd.HitTest().Row;
                int col = grd.HitTest().Column;

                string contents = string.Empty;
                try
                {
                    contents = grd[row, col].ToString();
                }
                catch (Exception)
                {

                    return;
                }

                if (row <= 0 || string.IsNullOrEmpty(contents))
                {
                    return;
                }
                

                try
                {
                    //String str = Clipboard.GetText();
                    //add delimiters as per your requirement.
                    //add [backslash]t to paste cell data in single row in ms-excel
                    //Clipboard.SetText(str + grd[row, col].ToString() + "[backslash]t");
                    Clipboard.Clear();

                    Clipboard.SetText(grd[row, col].ToString());
                }
                catch (Exception)
                {

                    return;

                }
            }

        }

        public void FlexGridRow(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRowsHeight, int DataRowsHeight)
        {
            grdItem.Rows[0].Height = TopRowsHeight;

            for (int i = 1; i < grdItem.Rows.Count; i++)
                grdItem.Rows[i].Height = DataRowsHeight;
        }

        public void FlexGridRow(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int DataRowsHeight)
        {
            for (int i = 1; i < grdItem.Rows.Count; i++)
                grdItem.Rows[i].Height = DataRowsHeight;
        }

        public void FlexGridRow(C1.Win.C1FlexGrid.C1FlexGrid grdItem, int TopRowsHeight, int CaptionRowHeight, int DataRowsHeight)
        {
            grdItem.Rows[0].Height = TopRowsHeight;
            for (int i = 1; i < grdItem.Rows.Fixed + grdItem.Rows.Frozen; i++)
                grdItem.Rows[i].Height = CaptionRowHeight;

            for (int i = grdItem.Rows.Fixed + grdItem.Rows.Frozen; i < grdItem.Rows.Count; i++)
                grdItem.Rows[i].Height = DataRowsHeight;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dt)
        {
            int intRowNum = 0;

            int NewRowsCount = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;

            try
            {
                if(grdItem.Rows.Count < NewRowsCount)
                { 
                    grdItem.Rows.Count = NewRowsCount;
                }
                while (dt.Rows.Count > intRowNum)
                {
                    int intRow = intRowNum + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                    DataRow dr = dt.Rows[intRowNum];
                    for (int intCol = grdItem.Cols.Fixed + grdItem.Cols.Frozen; intCol < dt.Columns.Count; intCol++)
                    {
                        grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                        grdItem[intRow, intCol] = dr.ItemArray[intCol];
                    }

                    intRowNum++;
                }

                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dt, bool bBinding)
        {
            int intRowNum = 0;

            int NewRowsCount = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;

            try
            {
                if (bBinding)
                {
                    for (int intCol = grdItem.Cols.Fixed + grdItem.Cols.Frozen; intCol < dt.Columns.Count; intCol++)
                    {
                        grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                    }

                    grdItem.SetDataBinding(dt, "", true);
                    intRowNum = dt.Rows.Count;
                }
                else
                {
                    if (grdItem.Rows.Count < NewRowsCount)
                    {
                        grdItem.Rows.Count = NewRowsCount;
                    }

                    while (dt.Rows.Count > intRowNum)
                    {
                        int intRow = intRowNum + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                        DataRow dr = dt.Rows[intRowNum];
                        for (int intCol = grdItem.Cols.Fixed + grdItem.Cols.Frozen; intCol < dt.Columns.Count; intCol++)
                        {
                            grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                            grdItem[intRow, intCol] = dr.ItemArray[intCol];
                        }

                        intRowNum++;
                    }
                }
                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dt, int StartRowIndex, int StartColumnIndex, bool bBinding=false)
        {
            int intRowNum = 0;

            try
            {
                for (int intCol = StartColumnIndex; intCol < dt.Columns.Count; intCol++)
                {
                    grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                }

                if (bBinding)
                {
                    grdItem.SetDataBinding(dt, "", true);
                    intRowNum = dt.Rows.Count;
                }
                else
                {
                    grdItem.Rows.Count = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                    while (dt.Rows.Count > intRowNum)
                    {
                        int intRow = intRowNum + StartRowIndex;
                        DataRow dr = dt.Rows[intRowNum];
                        for (int intCol = StartColumnIndex; intCol < dt.Columns.Count; intCol++)
                        {
                            grdItem[intRow, intCol] = dr.ItemArray[intCol];
                        }

                        intRowNum++;
                    }
                }

                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dt, FlexGridColumnCaptionEnum ColumnCaptionEnum, FlexGridRowCaptionEnum RowCaptionEnum, bool bBinding=false)
        {
            int intRowNum = 0;
            int intRow = 0;
            int intCol = 0;

            if(ColumnCaptionEnum == FlexGridColumnCaptionEnum.Fixed)
            {
                intCol = grdItem.Cols.Fixed;
            }
            else if(ColumnCaptionEnum == FlexGridColumnCaptionEnum.None)
            {
                intCol = 0;
            }
            else if (ColumnCaptionEnum == FlexGridColumnCaptionEnum.Frozen)
            {
                intCol = grdItem.Cols.Frozen;
            }
            else 
            {
                intCol = grdItem.Cols.Fixed + grdItem.Cols.Frozen;
            }

            if (RowCaptionEnum == FlexGridRowCaptionEnum.Fixed)
            {
                intRow = grdItem.Rows.Fixed;
            }
            else if (RowCaptionEnum == FlexGridRowCaptionEnum.None)
            {
                intRow = 0;
            }
            else if (RowCaptionEnum == FlexGridRowCaptionEnum.Frozen)
            {
                intRow = grdItem.Rows.Frozen;
            }
            else
            {
                intRow = grdItem.Rows.Fixed + grdItem.Rows.Frozen;
            }

            try
            {
                while (intCol < dt.Columns.Count)
                {
                    grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                }

                if(bBinding)
                {
                    grdItem.SetDataBinding(dt, "", true);
                    intRowNum = dt.Rows.Count;
                }
                else
                {
                    grdItem.Rows.Count = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                    while (dt.Rows.Count > intRowNum)
                    {
                        intRow = intRow + intRowNum;
                        DataRow dr = dt.Rows[intRowNum];
                        while (intCol < dt.Columns.Count)
                        {
                            grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;
                            grdItem[intRow, intCol] = dr.ItemArray[intCol];
                            intCol++;
                        }

                        intRowNum++;
                    }
                }

                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }

        public void grdDataClear(C1FlexGrid grdItem, int minRowsCount)
        {
            while (grdItem.Rows.Count > minRowsCount)
            {
                grdItem.Rows.Remove(minRowsCount);
            }
        }

        public void grdDataClearForBind(C1FlexGrid grdItem)
        {
            int defalutRowCount = 2;
            while (grdItem.Rows.Count > defalutRowCount)
            {
                grdItem.Rows.Remove(defalutRowCount);
            }
        }





        public void grdDataClearForNotBind(C1FlexGrid grdItem)
        {
            int defalutRowCount = 2;
            grdItem.Rows.RemoveRange(defalutRowCount, grdItem.Rows.Count - defalutRowCount);
        }

        public bool CanRowSelectGrid(C1FlexGrid grdMain, int _mingridRows)
        {
            if (grdMain.Rows.Count > _mingridRows) return true;

            return false;
        }

        public bool CanMoveRow(C1FlexGrid grd, int _dragitemRow, int _dropitemRow, int compareIndex)
        {
            bool rtn = false;

            if (grd.Rows[_dragitemRow][compareIndex].ToString().Equals(grd.Rows[_dropitemRow][compareIndex].ToString()))
            {
                rtn = true;
            }
            return rtn;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, DataTable dt, clsFlexGridColumns Columns, bool bBinding=false)
        {
            int intRowNum = 0;
            StringBuilder strData = new StringBuilder();

            try
            {
                for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                {
                    grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;

                    if (Columns == null) continue;

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        if (dt.Columns[intCol].ColumnName == Columns.GetName(i).ToString())
                        {
                            if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.CheckBox)
                            {
                                grdItem.Cols[intCol].DataType = typeof(Boolean);
                            }
                            else if(Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.ComboBox)
                            {
                                grdItem.Cols[intCol].AllowEditing = true;


                                if(Columns.GetDataMap(i).Count > 0)
                                {
                                    ListDictionary DataMap = Columns.GetDataMap(i);
                                    grdItem.Cols[intCol].DataMap = DataMap;
                                    grdItem.Cols[intCol].TextAlign = TextAlignEnum.CenterCenter;

                                }
                                else if (Columns.GetComboList(i).Count > 0)
                                {
                                    ArrayList arrData = Columns.GetComboList(i);
                                    for (int j = 0; j < Columns.GetComboList(i).Count; j++)
                                        strData.Append(arrData[j].ToString() + "|");

                                    grdItem.Cols[intCol].ComboList = strData.ToString();
                                }
                            }
                            else if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.TimePicker)
                            {
                                grdItem.Cols[intCol].DataType = typeof(DateTime);
                                
                                DataGridCenterStyle(grdItem, intCol, intCol);

                            }
                            else if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.TimePicker_All)
                            {
                                grdItem.Cols[intCol].DataType = typeof(DateTime);
                                grdItem.Cols[intCol].Format = "yyyy-MM-dd HH:mm:ss";
                                DataGridCenterStyle(grdItem, intCol, intCol);

                            }
                        }

                    }
                }

                if (bBinding)
                {
                    grdItem.SetDataBinding(dt, "", true);
                    intRowNum = dt.Rows.Count;
                }
                else
                {
                    grdItem.Rows.Count = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                    while (dt.Rows.Count > intRowNum)
                    {
                        int intRow = intRowNum + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                        DataRow dr = dt.Rows[intRowNum];
                        for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                        {
                            grdItem[intRow, intCol] = dr.ItemArray[intCol];
                        }

                        intRowNum++;
                    }
                }

                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }

        public int FlexGridBinding(C1.Win.C1FlexGrid.C1FlexGrid grdItem, clsFlexGridColumns Columns)
        {
            int intRowNum = 0;
            DataTable dt = (DataTable)grdItem.DataSource;
            StringBuilder strData = new StringBuilder();

            try
            {
                for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                {
                    grdItem.Cols[intCol].Name = dt.Columns[intCol].ColumnName;

                    if (Columns == null) continue;

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        if (dt.Columns[intCol].ColumnName == Columns.GetName(i).ToString())
                        {
                            if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.CheckBox)
                            {
                                grdItem.Cols[intCol].DataType = typeof(Boolean);
                            }
                            else if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.ComboBox)
                            {
                                grdItem.Cols[intCol].AllowEditing = true;


                                if (Columns.GetDataMap(i).Count > 0)
                                {
                                    ListDictionary DataMap = Columns.GetDataMap(i);
                                    grdItem.Cols[intCol].DataMap = DataMap;
                                    grdItem.Cols[intCol].TextAlign = TextAlignEnum.CenterCenter;

                                }
                                else if (Columns.GetComboList(i).Count > 0)
                                {
                                    ArrayList arrData = Columns.GetComboList(i);
                                    for (int j = 0; j < Columns.GetComboList(i).Count; j++)
                                        strData.Append(arrData[j].ToString() + "|");

                                    grdItem.Cols[intCol].ComboList = strData.ToString();
                                }
                            }
                            else if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.TimePicker)
                            {
                                grdItem.Cols[intCol].DataType = typeof(DateTime);

                                DataGridCenterStyle(grdItem, intCol, intCol);

                            }
                            else if (Columns.GetCellTypeEnum(i) == FlexGridCellTypeEnum.TimePicker_All)
                            {
                                grdItem.Cols[intCol].DataType = typeof(DateTime);
                                grdItem.Cols[intCol].Format = "yyyy-MM-dd HH:mm:ss";
                                DataGridCenterStyle(grdItem, intCol, intCol);

                            }
                        }

                    }
                }

                //if (bBinding)
                //{
                //    grdItem.SetDataBinding(dt, "", true);
                //    intRowNum = dt.Rows.Count;
                //}
                //else
                //{
                //    grdItem.Rows.Count = dt.Rows.Count + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                //    while (dt.Rows.Count > intRowNum)
                //    {
                //        int intRow = intRowNum + grdItem.Rows.Fixed + grdItem.Rows.Frozen;
                //        DataRow dr = dt.Rows[intRowNum];
                //        for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                //        {
                //            grdItem[intRow, intCol] = dr.ItemArray[intCol];
                //        }

                //        intRowNum++;
                //    }
                //}

                return intRowNum;
            }
            catch (Exception ex)
            {
            }

            return intRowNum;
        }
    }
    
    public class clsFlexGridColumns
    {
        private ArrayList FlexGridColumn;

        private class clsFlexGridColumnValue
        {
            //private string Name;
            public object Name;
            public FlexGridCellTypeEnum CellTypeEnum;
            public object CheckBoxData;
            public ArrayList ComboList;
            public ListDictionary DataMap;
            public DateTime DateTimeData;

            public clsFlexGridColumnValue(object objName, FlexGridCellTypeEnum CellTypeEnumValue, ArrayList DataListValue)
            {
                Name = objName;
                CellTypeEnum = CellTypeEnumValue;
                ComboList = DataListValue;
            }

            public clsFlexGridColumnValue(object objName, FlexGridCellTypeEnum CellTypeEnumValue, object CheckBoxDataValue)
            {
                Name = objName;
                CellTypeEnum = CellTypeEnumValue;
                CheckBoxData = CheckBoxDataValue;
            }

            public clsFlexGridColumnValue(object objName, FlexGridCellTypeEnum CellTypeEnumValue, ListDictionary ComboBoxDataMap)
            {
                Name = objName;
                CellTypeEnum = CellTypeEnumValue;
                DataMap = ComboBoxDataMap;
            }
            public clsFlexGridColumnValue(object objName, FlexGridCellTypeEnum CellTypeEnumValue, DateTime _DateTimeData)
            {
                Name = objName;
                CellTypeEnum = CellTypeEnumValue;
                DateTimeData = _DateTimeData;
            }
        }

        public clsFlexGridColumns()
        {
            FlexGridColumn = new ArrayList();
        }

        public int Count
        {
            get { return FlexGridColumn.Count; }
        }

        public void Add(object objName, FlexGridCellTypeEnum CellTypeEnumValue, ArrayList DataListValue)
        {
            this.FlexGridColumn.Add(new clsFlexGridColumnValue(objName, CellTypeEnumValue, DataListValue));
        }

        public void Add(object objName, FlexGridCellTypeEnum CellTypeEnumValue, object CheckBoxData)
        {
            this.FlexGridColumn.Add(new clsFlexGridColumnValue(objName, CellTypeEnumValue, CheckBoxData));
        }

        public void Add(object objName, FlexGridCellTypeEnum CellTypeEnumValue, ListDictionary DataMap)
        {
            this.FlexGridColumn.Add(new clsFlexGridColumnValue(objName, CellTypeEnumValue, DataMap));
        }

        public void Add(object objName, FlexGridCellTypeEnum CellTypeEnumValue, DateTime Datetime)
        {
            this.FlexGridColumn.Add(new clsFlexGridColumnValue(objName, CellTypeEnumValue, Datetime));
        }

        public void Clear()
        {
            this.FlexGridColumn.Clear();
        }

        public void Remove(string item_name)
        {
            this.FlexGridColumn.Remove(item_name);
        }

        public object GetName(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.Name;
        }

        public FlexGridCellTypeEnum GetCellTypeEnum(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.CellTypeEnum;
        }

        public ArrayList GetComboList(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.ComboList;
        }


        public ListDictionary GetDataMap(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.DataMap;
        }

        public object GetCheckBoxData(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.CheckBoxData;
        }

        public object GetDateTimeData(int index)
        {
            clsFlexGridColumnValue pv = (clsFlexGridColumnValue)this.FlexGridColumn[index];
            return pv.DateTimeData;
        }


    }

    
}


