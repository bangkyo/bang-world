using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ComLib
{
    public class SqlConnectDB
    {
        //clsStyle cs = new clsStyle();

        private bool IsDBAvailable = false;

        //계발계
        //string ConnString = "Data Source=(DESCRIPTION ="
        //                  + "(ADDRESS_LIST=(ADDRESS = (PROTOCOL = TCP)(HOST = 10.216.80.248)(PORT = 1521)))"
        //                  + "(CONNECT_DATA=(SERVER = 10.216.80.248)(SERVICE_NAME = L2PRB)));"
        //                  + "User Id=l2user_dev;Password=l2user_dev;";
        //실서버
        string ConnString = "Data Source=(DESCRIPTION ="
                          + "(ADDRESS_LIST=(ADDRESS = (PROTOCOL = TCP)(HOST = 10.216.80.248)(PORT = 1521)))"
                          + "(CONNECT_DATA=(SERVER = 10.216.80.248)(SERVICE_NAME = L2PRB)));"
                         + "User Id=l2user;Password=l2user;";


        public bool CheckDbConnection()
        {
            try
            {
                using (var connection = new SqlConnection(ConnString))
                {
                    connection.Open();
                    IsDBAvailable = true;
                    return IsDBAvailable;
                }
            }
            catch (Exception ex)
            {
                //logger.Warn(LogTopicEnum.Agent, "Error in DB connection test on CheckDBConnection", ex);
                IsDBAvailable = false;
                return IsDBAvailable; // any error is considered as db connection error for now
            }
        }
        public SqlConnection OConnect()
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConnString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("■OConnect Error■\n" + ex.Message);
            }
            finally
            {
                if (conn != null) { conn.Close(); }
            }

            return conn;
        }

        public DataTable FindDataTable(string Sql)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
            
                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                //MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        /// <summary>
        /// 쿼리 변수 처리 변수앞에는 ' : ' 사용한다.( :userId ) 
        /// 파라메타는 string[] 으로 보낸다 중간 구분자는 ' | ' 이용한다. ( ":userId|AAAA") 
        /// </summary>
        /// <param name="Sql">쿼리문</param>
        /// <param name="parm">파라메타 배열</param>
        /// <returns></returns>
        public DataTable FindDataTable(string Sql, string[] parm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();
            SqlParameter op = null;

            try
            {
                string[] split;

                conn.Open();
                ocmd = new SqlCommand(Sql, conn);

                for (int ii = 0; ii < parm.Length; ii++)
                {
                    if (parm[ii] != null && parm[ii] != "")
                    {
                        split = parm[ii].Split('|');

                        op = new SqlParameter(split[0], split[1]);

                        ocmd.Parameters.Add(op);
                    }
                }

                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        /// <summary>
        /// 쿼리 실행 (입력, 수정, 삭제)
        /// </summary>
        /// <param name="Sql">쿼리문</param>
        /// <returns>int (0:실패, 1이상:성공)</returns>
        public int ExecuteQuery(string Sql)
        {
            SqlConnection conn = OConnect();
            SqlCommand ocmd = null;
            int ridx = 0;

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                ridx = ocmd.ExecuteNonQuery();
                //AutoCommit
                //ocmd = new SqlCommand("commit", conn);
                //ocmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("■ExecuteQuery Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (ocmd != null) { ocmd.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return ridx;
        }

        /// <summary>
        /// 다중 쿼리 실행
        /// </summary>
        /// <param name="Sql">쿼리문 배열</param>
        /// <returns>에러시 값있음</returns>
        public bool ExecuteQuery(string[] Sql)
        {
            SqlConnection conn = OConnect();
            SqlCommand ocmd = new SqlCommand();
            SqlTransaction tran = null;
            bool boolRet = false;

            try
            {
                conn.Open();
                ocmd.Connection = conn;
                tran = conn.BeginTransaction();
                ocmd.Transaction = tran;

                for (int ii = 0; ii < Sql.Length; ii++)
                {
                    if (Sql[ii] != null && Sql[ii] != "")
                    {
                        ocmd.CommandText = Sql[ii];
                        ocmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();

                boolRet = true;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }

                string msg = "■ExecuteQuery Error■\n" + ex.Message + "\n■Sql■\n";

                for (int ii = 0; ii < Sql.Length; ii++)
                {
                    msg += ii + "=[" + Sql[ii] + "]\n";
                }

                MessageBox.Show(msg);
            }
            finally
            {
                if (ocmd != null) { ocmd.Dispose(); }
                if (tran != null) { tran.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return boolRet;
        }

        /// <summary>
        /// 값이 있으면 true 이다
        /// </summary>
        /// <param name="Sql">쿼리</param>
        /// <returns>bool</returns>
        public bool IsInsUpChk(string Sql)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();
            bool boolRet = false;

            try
            {
                conn.Open();
                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                boolRet = (dt.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return boolRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, string colNm)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colNm">컬럼명</param>
        /// <param name="nullDefault">null 이거나 없을시</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, string colNm, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][colNm].ToString() != "")
                {
                    strRet = dt.Rows[0][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int colIndex)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[0][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Column Index : " + colIndex);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, string colNm)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[rowIndex][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, int colIndex)
        {
            string strRet = "";

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strRet = dt.Rows[rowIndex][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Index : " + colIndex);
            }

            return strRet;
        }



        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colNm">컬럼명</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, string colNm, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[rowIndex][colNm].ToString() != "")
                {
                    strRet = dt.Rows[rowIndex][colNm].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Name : " + colNm);
            }

            return strRet;
        }

        /// <summary>
        /// 데이타테이블에 컬럼명 받아오기
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rowIndex">로우인덱스</param>
        /// <param name="colIndex">컬럼인덱스</param>
        /// <returns></returns>
        public string getColValue(DataTable dt, int rowIndex, int colIndex, string nullDefault)
        {
            string strRet = nullDefault;

            try
            {
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[rowIndex][colIndex].ToString() != "")
                {
                    strRet = dt.Rows[rowIndex][colIndex].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("■getColValue Error■\n" + ex.Message + "\n■Row Index : " + rowIndex + ", Column Index : " + colIndex);
            }

            return strRet;
        }

        public string Find_CD_NM(string category, string cd_id)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT  CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and CD_ID = '{1}' AND USE_YN = 'Y' ORDER BY SORT_SEQ, CD_NM", category, cd_id);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }
                
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public string Find_CD_ID(string category, string cd_nm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and CD_NM = '{1}' AND USE_YN = 'Y' ORDER BY SORT_SEQ, CD_NM, CD_ID", category, cd_nm);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }
                
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public DataTable Find_CD(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' AND USE_YN = 'Y'  ORDER BY SORT_SEQ, CD_ID", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];

                
                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_ROUTING_CD(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                string Sql1 = string.Format("SELECT CD_ID,   ");
                Sql1 += string.Format("  CD_NM    ");
                Sql1 += string.Format("FROM TB_CM_COM_CD  ");
                Sql1 += string.Format("WHERE CATEGORY = 'ROUTING_CD' ");
                Sql1 += string.Format("   AND CD_ID = 'A1'           ");
                Sql1 += string.Format("   OR CD_ID = 'B1'   ");
                Sql1 += string.Format("   OR CD_ID = 'L3'  ");
                Sql1 += string.Format("ORDER BY CD_ID   ");

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_TC_CD(string category , string line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                string Sql1 = string.Format("SELECT CD_ID, ");
                Sql1 += string.Format("CD_NM    ");
                Sql1 += string.Format("FROM TB_CM_COM_CD  ");
                Sql1 += string.Format("WHERE CATEGORY = 'TC_CD' AND USE_YN = 'Y'");
                if (line_gp != "%")
                {
                    Sql1 += string.Format("AND COLUMNA = '{0}' ", line_gp);
                }
                Sql1 += string.Format("ORDER BY SORT_SEQ  ");

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable Find_CD_GOOD_NG(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT CD_ID, CD_NM  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}'  ORDER BY SORT_SEQ, CD_ID", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }

        public DataTable GetScreeAcl(string UserGrp)
        {
            DataTable dt = null;

            string Sql1 = string.Empty;

            Sql1 += string.Format("SELECT ");
            Sql1 += string.Format("    B.PAGE_ID ");
            Sql1 += string.Format("   ,NVL(A.INQ_ACL,'N') AS INQ_ACL ");
            Sql1 += string.Format("   ,NVL(A.REG_ACL,'N') AS REG_ACL ");
            Sql1 += string.Format("   ,NVL(A.MOD_ACL,'N') AS MOD_ACL ");
            Sql1 += string.Format("   ,NVL(A.DEL_ACL,'N') AS DEL_ACL ");
            Sql1 += string.Format("FROM TB_CM_ACL A, TB_CM_SCR B ");
            Sql1 += string.Format("WHERE ");
            Sql1 += string.Format("    A.ACL_GRP_ID = '{0}'", UserGrp);
            Sql1 += string.Format("AND A.SCR_ID = B.SCR_ID ");
            Sql1 += string.Format("AND B.USE_YN = 'Y' ");

            dt = FindDataTable(Sql1);

            return dt;
        }
        public bool SetCombo(ComboBox cb, string categoryNm)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                List<string> list = new List<string>();
                //list.Add("전체");

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    list.Add(dt.Rows[row].ItemArray[1].ToString());
                }
                foreach (var item in list)
                {
                    cb.Items.Add(item);
                }
                if (dt.Rows.Count > 0)
                {
                    //첫번째 아이템 선택
                    cb.SelectedIndex = 0;
                }
                
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// combobox setup
        /// </summary>
        /// <param name="cb"> combobox object</param>
        /// <param name="categoryNm"> category key string</param>
        /// <param name="ExtractItemNM"> 제거해야할 항목</param>
        /// <param name="AddTotal">전체를 추가할것인지</param>
        /// <returns></returns>
        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }
                    
                    
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
            
        }

        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool SetCombo_3(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 2;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //특정 항목들을 제거하기위한 설정
        public bool SetCombo(ComboBox cb, string categoryNm,  List<string> Extractlist, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                bool exists = false;
                foreach (DataRow row in dt.Rows)
                {

                    //리스트에서 해당 항목의 이름이 같은 경우가 있는지 체크
                    
                    //string result = Extractlist.Find(s => s.StartsWith(row["CD_NM"].ToString()));
                    exists = Extractlist.Exists(element => element == row["CD_NM"].ToString());

                    // 해당 항목이 제외할 아이템리스트에 없는경우  ==> 추가해야할경우
                    if (!exists)
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }
                    // 해당 항목이  제외할 아이템리스트에 없는경우 ==> 추가해야하지 않아야할경우

                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }


        //교정, 면취의 정보만 가진 공정 cb
        public bool SetCombo(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal, List<string> list)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["CD_NM"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());

                            
                        }
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            if (row["CD_NM"].ToString() == item)
                            {
                                arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());

                            }

                        }
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        public bool SetCombo(ComboBox cb, string categoryNm, bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();
                
                DataTable dt = Find_CD(categoryNm);


                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public void SetComboYN(ComboBox cb)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                ArrayList arrType1 = new ArrayList();

                arrType1.Add(new DictionaryList("Y", "Y"));
                arrType1.Add(new DictionaryList("N", "N"));


                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                return ;
            }

            return ;
        }

        public void SetComboHEATPOC(ComboBox cb)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();

                ArrayList arrType1 = new ArrayList();

                arrType1.Add(new DictionaryList("POC", "A"));
                arrType1.Add(new DictionaryList("HEAT", "B"));


                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                return;
            }

            return;
        }

        public bool SetCombo_TC_CD(ComboBox cb, string categoryNm, string line_gp ,  bool AddTotal, string selected_id)
        {
            try
            {
                // ComBoBox
                cb.DataSource = null;
                cb.Items.Clear();

                DataTable dt = Find_TC_CD(categoryNm,line_gp);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }

                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_ID"].ToString() +"  "+ row["CD_NM"].ToString(), row["CD_ID"].ToString()));
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
                //cb.Selecteditem = ck.StrKey2;

                foreach (var item in cb.Items)
                {
                    if (((DictionaryList)item).fnValue == selected_id)
                    {
                        cb.SelectedIndex = cb.Items.IndexOf(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public bool SetComboTC_CD(ComboBox cb, string categoryNm, string ExtractItemNM, bool AddTotal)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();

                if (AddTotal)
                {
                    arrType1.Add(new DictionaryList("전체", "%"));
                }


                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(ExtractItemNM))
                    {
                        if (row["COLUMNA"].ToString() != ExtractItemNM)
                        {
                            arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                        }
                    }
                    else
                    {
                        arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                    }


                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        public DataTable Find_STEEL_GRP(string category)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT DISTINCT  COLUMNA FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' AND USE_YN = 'Y' ORDER BY COLUMNA ", category);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string Find_STEEL_GRP(string category, string grp_nm)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;
            string rtn = string.Empty;

            try
            {
                conn.Open();

                Sql = string.Format("SELECT DISTINCT COLUMNA  FROM TB_CM_COM_CD WHERE CATEGORY = '{0}' and COLUMNA = '{1}' AND USE_YN = 'Y' ORDER BY COLUMNA", category, grp_nm);

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");

                dt = dset.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                {
                    rtn = "";
                }

                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return rtn;
        }

        public void SetComboIDNM(ComboBox cb, string categoryNm)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);

                cb.Items.Clear();
                DataTable dt = Find_CD(categoryNm);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["CD_NM"].ToString(), row["CD_ID"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                //cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
            }
            return ;
        }

        public void SetComboZoneInfo(ComboBox cb, string _line_gp)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();
                DataTable dt = FindReWorkZoneInfo(_line_gp);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["ZONE_CD"].ToString(), row["ZONE_CD"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                //첫번째 아이템 선택
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            return;
        }

        public void SetComboPOC(ComboBox cb, string _line_gp)
        {
            try
            {
                // ComBoBox
                //cboLine_GP.DataSource = new BindingSource(clsAdo.Ado.ExecuteQry("select distinct LINE_GP from TB_CR_PIECE_WR order by LINE_GP desc").Tables[0].DefaultView, null);
                cb.DataSource = null;
                cb.Items.Clear();
                DataTable dt = FindPOCInfo(_line_gp);

                ArrayList arrType1 = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    arrType1.Add(new DictionaryList(row["POC_NO"].ToString(), row["POC_NO"].ToString())); //.Add(row["CD_ID"].ToString(), row["CD_NM"].ToString());
                }

                cb.DataSource = arrType1;
                cb.DisplayMember = "fnText";
                cb.ValueMember = "fnValue";

                cb.DropDownStyle = ComboBoxStyle.DropDownList;

                if (dt.Rows.Count > 0)
                {
                    //첫번째 아이템 선택
                    cb.SelectedIndex = 0;
                }
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
            return;
        }

        public DataTable Find_BIZ_GP()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format("SELECT DISTINCT BIZ_GP  FROM TB_CM_SCR  ORDER BY BIZ_GP");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public DataTable Find_ACL_GRP_ID()
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql = string.Empty;

            try
            {
                conn.Open();
                Sql = string.Format(" SELECT ACL_GRP_ID,ACL_GRP_NM  FROM TB_CM_ACL_GRP ");

                ocmd = new SqlCommand(Sql, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string Find_Steel_NM_By_ID(string steel_id)
        {
            string rtn = "";

            string strQry = string.Empty;

            strQry += string.Format("select  ");
            strQry += string.Format("    ROWNUM AS L_NO");
            strQry += string.Format("   ,CD_NM ");
            strQry += string.Format("FROM  ");
            strQry += string.Format("    TB_CM_COM_CD  ");
            strQry += string.Format("WHERE  ");
            strQry += string.Format("    category = 'STEEL' ");
            strQry += string.Format("AND  ");
            strQry += string.Format("        CD_ID LIKE '{0}'  ", steel_id);
            strQry += string.Format("AND ROWNUM = 1      ");

            DataTable olddt = FindDataTable(strQry);

            if (olddt.Rows.Count > 0)
            {
                rtn = olddt.Rows[0]["CD_NM"].ToString();
            }

            return rtn;
        }

        public string Find_Scr_ID_By_PAGE_ID(string page_id)
        {
            string rtn = "";

            string strQry = string.Empty;

            strQry += string.Format("select  ");
            strQry += string.Format("    ROWNUM AS L_NO");
            strQry += string.Format("   ,SCR_ID ");
            strQry += string.Format("FROM  ");
            strQry += string.Format("    TB_CM_SCR  ");
            strQry += string.Format("WHERE  ");
            strQry += string.Format("    PAGE_ID = '{0}' ", page_id);
            strQry += string.Format("AND ROWNUM = 1      ");

            DataTable olddt = FindDataTable(strQry);

            if (olddt.Rows.Count > 0)
            {
                rtn = olddt.Rows[0]["SCR_ID"].ToString();
            }

            return rtn;
        }

        public DataTable FindReWorkZoneInfo(string _line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql1 = string.Empty;

            try
            {
                conn.Open();
                Sql1 += string.Format("SELECT ZONE_CD ");
                Sql1 += string.Format("FROM TB_CR_ZONEINFO ");
                Sql1 += string.Format("WHERE REENT_PSB_YN = 'Y' ");
                Sql1 += string.Format("and   LINE_GP  = '{0}' ", _line_gp);

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql1);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }


        public string GetPOCSql()
        {
            // --실시간트레킹중간_POC정보
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL),STEEL) AS STEEL_NM ");
            Sql1 += string.Format("      ,A.HEAT ");
            Sql1 += string.Format("      ,ITEM_SIZE ");
            Sql1 += string.Format("      ,LENGTH ");
            Sql1 += string.Format("      ,SURFACE_LEVEL ");
            Sql1 += string.Format("      ,POC_NO ");
            Sql1 += string.Format("      ,(SELECT SUM(PCS) FROM TB_CR_ORD_BUNDLEINFO WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS MILL_PCS   "); //--압연본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'A1' "); // 교정
            Sql1 += string.Format("        AND   LINE_GP    = :P_LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS STR_PCS   "); //--투입본수
            Sql1 += string.Format("      ,(SELECT NVL(SUM(PCS), 0) FROM TB_BND_WR ");
            Sql1 += string.Format("        WHERE POC_NO           = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   NVL(DEL_YN,'N') <> 'Y' ");
            Sql1 += string.Format("        AND   LINE_GP          = A.LINE_GP) AS OK_PCS   ");  //--합격본수, 바인딩본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'W2' ");
            Sql1 += string.Format("        AND   LINE_GP    = A.LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS NG_PCS    ");  //--불합격본수
            Sql1 += string.Format("FROM    ( ");
            Sql1 += string.Format("         SELECT POC_NO ");
            Sql1 += string.Format("               ,POC_SEQ ");
            Sql1 += string.Format("               ,STEEL ");
            Sql1 += string.Format("               ,HEAT ");
            Sql1 += string.Format("               ,ITEM_SIZE ");
            Sql1 += string.Format("               ,LENGTH ");
            Sql1 += string.Format("               ,SURFACE_LEVEL ");
            Sql1 += string.Format("               ,LINE_GP ");
            Sql1 += string.Format("         FROM   TB_CR_INPUT_ORD ");
            Sql1 += string.Format("         WHERE  (POC_NO, POC_SEQ) IN ( ");
            Sql1 += string.Format("                                       SELECT /*+RULE */ DISTINCT POC_NO, POC_SEQ ");
            Sql1 += string.Format("                                       FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                                       WHERE  PROG_STAT   IN ('RUN','WAT', 'FIN') ");
            Sql1 += string.Format("                                       AND    LINE_GP     = :P_LINE_GP   ");
            Sql1 += string.Format("                                       AND    ZONE_CD  not in ( '3Z99','3Z39')   ");
            Sql1 += string.Format("                                       AND    ROWNUM      = 1   ");
            Sql1 += string.Format("                                      ) ");
            Sql1 += string.Format("        ) A ");

            return Sql1;
        }

        public string GetPOCSql_12()
        {
            // --실시간트레킹중간_POC정보
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT NVL((SELECT CD_NM FROM TB_CM_COM_CD WHERE CATEGORY = 'STEEL' AND CD_ID = A.STEEL),STEEL) AS STEEL_NM ");
            Sql1 += string.Format("      ,A.HEAT ");
            Sql1 += string.Format("      ,ITEM_SIZE ");
            Sql1 += string.Format("      ,LENGTH ");
            Sql1 += string.Format("      ,SURFACE_LEVEL ");
            Sql1 += string.Format("      ,POC_NO ");
            Sql1 += string.Format("      ,(SELECT SUM(PCS) FROM TB_CR_ORD_BUNDLEINFO WHERE POC_NO = A.POC_NO AND POC_SEQ = A.POC_SEQ) AS MILL_PCS   "); //--압연본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'A1' "); // 교정
            Sql1 += string.Format("        AND   LINE_GP    = :P_LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        ) AS STR_PCS   "); //--투입본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO           = A.POC_NO AND POC_SEQ = A.POC_SEQ ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'F2'");
            Sql1 += string.Format("        AND   LINE_GP          = A.LINE_GP ");  
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        AND   GOOD_YN    = 'OK'");
            Sql1 += string.Format("        ) AS OK_PCS   "); //--합격본수
            Sql1 += string.Format("      ,(SELECT NVL(COUNT(*), 0) FROM TB_CR_PIECE_WR W ");
            Sql1 += string.Format("        WHERE POC_NO     = A.POC_NO AND POC_SEQ = A.POC_SEQ");
            Sql1 += string.Format("        AND   ROUTING_CD = 'F2' ");
            Sql1 += string.Format("        AND   LINE_GP    = A.LINE_GP ");
            Sql1 += string.Format("        AND   REWORK_SEQ = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                            WHERE  MILL_NO    = W.MILL_NO ");
            Sql1 += string.Format("                            AND    PIECE_NO   = W.PIECE_NO ");
            Sql1 += string.Format("                            AND    LINE_GP    = W.LINE_GP ");
            Sql1 += string.Format("                            AND    ROUTING_CD = W.ROUTING_CD) ");
            Sql1 += string.Format("        AND   GOOD_YN = 'NG' ");
            Sql1 += string.Format("        ) AS NG_PCS    ");  //--불합격본수
            Sql1 += string.Format("FROM    ( ");
            Sql1 += string.Format("         SELECT POC_NO ");
            Sql1 += string.Format("               ,POC_SEQ ");
            Sql1 += string.Format("               ,STEEL ");
            Sql1 += string.Format("               ,HEAT ");
            Sql1 += string.Format("               ,ITEM_SIZE ");
            Sql1 += string.Format("               ,LENGTH ");
            Sql1 += string.Format("               ,SURFACE_LEVEL ");
            Sql1 += string.Format("               ,LINE_GP ");
            Sql1 += string.Format("         FROM   TB_CR_INPUT_ORD ");
            Sql1 += string.Format("         WHERE  (POC_NO, POC_SEQ) IN ( ");
            Sql1 += string.Format("                                       SELECT /*+RULE */ DISTINCT POC_NO, POC_SEQ ");
            Sql1 += string.Format("                                       FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                                       WHERE  PROG_STAT   IN ('RUN','WAT', 'FIN') ");
            Sql1 += string.Format("                                       AND    LINE_GP     = :P_LINE_GP   ");
            Sql1 += string.Format("                                       AND    ZONE_CD  not in ( '1Z41','2Z43')   ");
            Sql1 += string.Format("                                       AND    ROWNUM      = 1   ");
            Sql1 += string.Format("                                      ) ");
            Sql1 += string.Format("        ) A ");

            return Sql1;
        }
        public string GetReadyBundleSql()
        {
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT B.MILL_NO ");
            Sql1 += string.Format("      ,A.PCS    AS MILL_PCS ");
            Sql1 += string.Format("      ,C.WR_PCS AS STR_PCS ");
            Sql1 += string.Format("FROM   TB_CR_INPUT_WR A ");
            Sql1 += string.Format("      ,(SELECT DISTINCT MILL_NO AS MILL_NO ");
            Sql1 += string.Format("        FROM   TB_RL_TM_TRACKING ");
            Sql1 += string.Format("        WHERE  PROG_STAT   IN ('RUN','WAT') ");
            Sql1 += string.Format("        AND    LINE_GP     =  :P_LINE_GP ");
            Sql1 += string.Format("        AND    ZONE_CD     LIKE '_Z01') B ");
            Sql1 += string.Format("      ,(SELECT  MILL_NO, COUNT(*) AS WR_PCS ");
            Sql1 += string.Format("        FROM   TB_CR_PIECE_WR P ");
            Sql1 += string.Format("        WHERE  LINE_GP     = :P_LINE_GP ");
            Sql1 += string.Format("        AND    ROUTING_CD  = 'A1' ");
            Sql1 += string.Format("        AND    REWORK_SEQ  = (SELECT MAX(REWORK_SEQ) FROM TB_CR_PIECE_WR ");
            Sql1 += string.Format("                              WHERE  MILL_NO    = P.MILL_NO ");
            Sql1 += string.Format("                              AND    PIECE_NO   = P.PIECE_NO ");
            Sql1 += string.Format("                              AND    LINE_GP    = P.LINE_GP ");
            Sql1 += string.Format("                              AND    ROUTING_CD = P.ROUTING_CD) ");
            Sql1 += string.Format("        GROUP BY MILL_NO ) C ");
            Sql1 += string.Format("WHERE  B.MILL_NO  = A.MILL_NO ");
            Sql1 += string.Format("AND    B.MILL_NO  = C.MILL_NO(+) ");
            Sql1 += string.Format("ORDER BY A.REG_DDTT, MILL_NO ");
            
            return Sql1;
        }

        public string GetZoneDataSql()
        {
            string Sql1 = string.Empty;
            //Sql1 += string.Format("SELECT ZONE_CD ");
            //Sql1 += string.Format("      ,MIN(MILL_NO) AS MILL_NO ");
            //Sql1 += string.Format("      ,COUNT(*)     AS PCS ");
            //Sql1 += string.Format("      ,MAX(BUNDLE_NO) AS BUNDLE_NO ");
            //Sql1 += string.Format("FROM   TB_RL_TM_TRACKING A ");
            //Sql1 += string.Format("WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            //Sql1 += string.Format("AND    NOT EXISTS (SELECT POC_NO FROM TB_CR_ORD WHERE POC_NO = A.POC_NO AND POC_PROG_STAT = 'D' )   ");
            //Sql1 += string.Format("AND    LINE_GP     =  :P_LINE_GP ");
            //Sql1 += string.Format("GROUP BY ZONE_CD ");

            Sql1 += string.Format("SELECT /*+RULE */ ZONE_CD ");
            Sql1 += string.Format("      ,MIN(MILL_NO) AS MILL_NO ");
            Sql1 += string.Format("      ,COUNT(*)     AS PCS ");
            Sql1 += string.Format("      ,MAX(BUNDLE_NO) AS BUNDLE_NO ");
            Sql1 += string.Format("      ,MAX(BUNDLE_PCS) AS BUNDLE_PCS ");
            Sql1 += string.Format("FROM   TB_RL_TM_TRACKING A ");
            Sql1 += string.Format("      ,( SELECT POC_NO, COUNT(*) AS BUNDLE_PCS FROM TB_BND_WR ");
            Sql1 += string.Format("         WHERE  POC_NO  = ( SELECT /*+RULE */ MAX(POC_NO) FROM TB_RL_TM_TRACKING ");
            Sql1 += string.Format("                            WHERE  PROG_STAT   IN ('RUN','FIN') ");
            Sql1 += string.Format("                            AND    ZONE_CD   <>  '3Z99' ");
            Sql1 += string.Format("                            AND    LINE_GP     =  :P_LINE_GP ) ");
            Sql1 += string.Format("         AND   NVL(DEL_YN,'N') <> 'Y' ");
            Sql1 += string.Format("          GROUP BY POC_NO ) B ");
            Sql1 += string.Format("WHERE  PROG_STAT   IN ('RUN','WAT','FIN') ");
            Sql1 += string.Format("AND    A.POC_NO    = B.POC_NO(+) ");
            Sql1 += string.Format("AND    LINE_GP     =  :P_LINE_GP ");
            Sql1 += string.Format("GROUP BY ZONE_CD ");

            return Sql1;
        }

        /// <summary>
        /// 해당 실시간 트레킹라인에서 현재 진행중인 데이터를 테이블로 가져옴
        /// </summary>
        /// <param name="line_gp"></param>
        /// <returns></returns>
        public DataTable GetRTDATA(string line_gp)
        {
            DataTable dt = new DataTable();

            try
            {
                string Sql1 = string.Empty;
                Sql1 += string.Format("SELECT MILL_NO ");
                Sql1 += string.Format("      ,PIECE_NO ");
                Sql1 += string.Format("      ,POC_NO ");
                Sql1 += string.Format("      ,PROG_STAT ");
                Sql1 += string.Format("FROM TB_RL_TM_TRACKING ");
                Sql1 += string.Format("WHERE LINE_GP = '{0}' ", line_gp);
                Sql1 += string.Format("AND   PROG_STAT = 'RUN' ");


                dt = FindDataTable(Sql1);

            }
            catch (Exception)
            {

            }

            return dt;
        }

        public DataTable FindPOCInfo(string _line_gp)
        {
            SqlConnection conn = OConnect();
            DataTable dt = null;
            SqlCommand ocmd = null;
            SqlDataAdapter olda = null;
            DataSet dset = new DataSet();

            string Sql1 = string.Empty;

            try
            {
                conn.Open();
                Sql1 = GetDistinctPOCSql(_line_gp);

                ocmd = new SqlCommand(Sql1, conn);
                olda = new SqlDataAdapter();
                olda.SelectCommand = ocmd;

                olda.Fill(dset, "DataTable");
                dt = dset.Tables[0];


                //rtn = dt.Rows[0].ItemArray[0].ToString().Trim();
                //rtn = dt.Rows[0].;

            }
            catch (Exception ex)
            {
                MessageBox.Show("■FindDataTable Error■\n" + ex.Message + "\n■Sql■\n" + Sql1);
            }
            finally
            {
                if (dset != null) { dset.Dispose(); }
                if (ocmd != null) { ocmd.Dispose(); }
                if (olda != null) { olda.Dispose(); }
                if (conn != null) { conn.Close(); }
            }

            return dt;
        }
        public string GetDistinctPOCSql(string _line_gp)
        {
            string Sql1 = string.Empty;
            Sql1 += string.Format("SELECT DISTINCT POC_NO ");
            Sql1 += string.Format("FROM TB_PROG_POC_MGMT ");
            Sql1 += string.Format("WHERE    ");
            Sql1 += string.Format("    LINE_GP     =  '{0}'    ", _line_gp);
            //Sql1 += string.Format("ORDER BY EXIT_DDTT DESC ");

            return Sql1;
        }

        public void InsertLogForSearch(string userid, Button btn)
        {
            string user_id = userid;
            string btn_gp = "1";
            string scr_id;

            string pageid = string.IsNullOrEmpty(btn.Parent.Parent.Parent.Name ) ? btn.Parent.Parent.Name: btn.Parent.Parent.Parent.Name;

            if (string.IsNullOrEmpty(Find_Scr_ID_By_PAGE_ID(pageid)))
            {
                return;
            }
            else
            {
                scr_id = Find_Scr_ID_By_PAGE_ID(pageid);
            }

            //디비선언
            SqlConnection conn = OConnect();

            SqlCommand cmd = new SqlCommand();
            SqlTransaction transaction = null;

            try
            {

                conn.Open();
                cmd.Connection = conn;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                //TB_CM_SCR_USEHIS -> MDMS.dbo.TB_CM_SCR_USEHIS
                string Sql1 = string.Empty;
                Sql1 += string.Format(" INSERT INTO TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("      ( ");
                Sql1 += string.Format("         USER_ID ");
                Sql1 += string.Format("        ,SCR_ID ");
                Sql1 += string.Format("        ,USE_DATE_SEQ ");
                Sql1 += string.Format("        ,USE_DDTT ");
                Sql1 += string.Format("        ,BTN_GP ");
                Sql1 += string.Format("        ,REGISTER ");
                Sql1 += string.Format("        ,REG_DDTT ");
                Sql1 += string.Format("       ) ");
                Sql1 += string.Format(" VALUES ( ");
                Sql1 += string.Format("           '{0}' ", user_id);
                Sql1 += string.Format("          ,'{0}' ", scr_id);
                Sql1 += string.Format("          ,( ");
                Sql1 += string.Format("             SELECT NVL(MAX(USE_DATE_SEQ),0) + 1 FROM  TB_CM_SCR_USEHIS ");
                Sql1 += string.Format("             WHERE  USER_ID = '{0}' ", user_id);
                Sql1 += string.Format("             AND    SCR_ID  = '{0}' ", scr_id);
                Sql1 += string.Format("             AND    TO_CHAR(USE_DDTT,'YYYYMMDD') = TO_CHAR((select convert(varchar, getdate(), 120)),'YYYYMMDD') ");
                Sql1 += string.Format("            ) ");
                Sql1 += string.Format("          , (select convert(varchar, getdate(), 120)) ");
                Sql1 += string.Format("          , '{0}' ", btn_gp);
                Sql1 += string.Format("          , '{0}' ", user_id);
                Sql1 += string.Format("          , (select convert(varchar, getdate(), 120)) ");
                Sql1 += string.Format("         ) ");

                cmd.CommandText = Sql1;
                cmd.ExecuteNonQuery();

                //실행후 성공
                transaction.Commit();

            }
            catch (System.Exception ex)
            {
                //실행후 실패 : 
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                // 추가되었을시에 중복성으로 실패시 메시지 표시
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();       //데이터베이스연결해제
                if (transaction != null)
                    transaction.Dispose();
            }
        }
    }
}
