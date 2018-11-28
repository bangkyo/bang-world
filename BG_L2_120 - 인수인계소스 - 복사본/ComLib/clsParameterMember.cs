using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLib
{

    //2016.06.07 OCJ 추가
    //DATABASE 관련 SQL문 읽을 때 조건문 파라메터 셋팅할 때 필요한 CLASS

    public class clsParameterMember
    {
        private ArrayList param;

        #region [Class] ParameterValue : ADO의 Parameter의 내용을 보관할 틀
        private class clsParameterValue
        {
            public object Type;
            public object Name;
            public object Value;
            public object Direction;
            public object Size;

            public clsParameterValue(object objType, object objName, object objValue, object objDirection)
            {
                this.Type = objType;
                this.Name = objName;
                this.Value = objValue;
                this.Direction = objDirection;
            }

            public clsParameterValue(object objType, object objName, object objValue, object objDirection, object objSize)
            {
                this.Type = objType;
                this.Name = objName;
                this.Value = objValue;
                this.Direction = objDirection;
                this.Size = objSize;
            }


            public clsParameterValue(object objType, object objName, object objValue)
            {
                this.Type = objType;
                this.Name = objName;
                this.Value = objValue;
            }

            public clsParameterValue(object objName, object objValue)
            {
                this.Name = objName;
                this.Value = objValue;
            }
        }
        #endregion

        #region 생성자
        public clsParameterMember()
        {
            param = new ArrayList();
        }
        #endregion

        #region [Method] Count : 등록된 Parameter의 개수를 리턴한다.
        public int Count
        {
            get { return param.Count; }
        }
        #endregion

        #region [Method] 현재 이름으로 등록한 Parameter의 index를 리턴한다.
        public int GetIndex(string name)
        {
            for(int i = 0; i < param.Count; i++)
            {
                if(((clsParameterValue)this.param[i]).Name.ToString() == name)
                {
                    return i;
                }
            }
            return 0;
        }
        #endregion

        #region [Method] Add : Parameter 등록
        public void Add(object objType, object objName, object objValue, object objDirection)
        {
            this.param.Add(new clsParameterValue(objType, objName, objValue, objDirection));
        }

        public void Add(object objType, object objName, object objValue, object objDirection, object objSize)
        {
            this.param.Add(new clsParameterValue(objType, objName, objValue, objDirection, objSize));
        }

        public void Add(object objType, object objName, object objValue)
        {
            this.param.Add(new clsParameterValue(objType, objName, objValue));
        }

        public void Add(object objName, object objValue)
        {
            this.param.Add(new clsParameterValue(objName, objValue));
        }
        #endregion

        #region [Method] Clear : Parameter Clear
        public void Clear()
        {
            this.param.Clear();
        }
        #endregion

        #region [Method] GetType : 해당 Index의 SqlDbType 반환
        public object GetType(int index)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            return pv.Type;
        }
        #endregion

        #region [Method] GetName : 해당 Index의 Parameter Name 반환
        public string GetName(int index)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            return pv.Name.ToString();
        }
        #endregion

        #region [Method] GetValue : 해당 Index/Name의 Parameter Value 반환
        public object GetValue(int index)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            return pv.Value;
        }

        public object GetValue(string name)
        {
            for (int i = 0; i < param.Count; i++)
                if (((clsParameterValue)this.param[i]).Name.ToString() == name)
                    return ((clsParameterValue)this.param[i]).Value;
            return null;
        }
        #endregion

        #region [Method] GetValue : 해당 Index/Name의 Parameter Direction 반환
        public object GetDirection(int index)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            return pv.Direction;
        }

        public object GetSize(int index)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            return pv.Size;
        }

        public object GetDirection(string name)
        {
            for (int i = 0; i < param.Count; i++)
                if (((clsParameterValue)this.param[i]).Name.ToString() == name)
                    return ((clsParameterValue)this.param[i]).Direction;
            return null;
        }
        #endregion

        #region [Method] SetValue : 해당 Index의 Parameter Value 설정
        public void SetValue(int index, object value)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            pv.Value = value;
        }
        #endregion

        #region [Method] SetValue : 해당 Index의 Parameter Value, Parameter Direction 설정
        public void SetValue(int index, object value, object Direction)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            pv.Value = value;
            pv.Direction = Direction;
        }
        #endregion

        #region [Method] SetValue : 해당 Index의 Parameter Value, Parameter Direction, Parameter Size 설정
        public void SetValue(int index, object value, object Direction, object Size)
        {
            clsParameterValue pv = (clsParameterValue)this.param[index];
            pv.Value = value;
            pv.Direction = Direction;
            pv.Size = Size;
        }
        #endregion
    }
}