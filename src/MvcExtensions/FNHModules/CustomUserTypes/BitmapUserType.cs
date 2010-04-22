using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using NHibernate;
using System.IO;
using System.Drawing;
using System.Data;
using FluentNHibernate.Conventions;
using System.Drawing.Imaging;

// based on : http://www.martinwilley.com/net/code/nhibernate/usertype.html

namespace MvcExtensions.FNHModules.CustomUserTypes
{
    public class BitmapUserType : IUserType
    {
        #region IUserType Members
 
        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }
 
        public object DeepCopy(object value)
        {
            return value;
        }
 
        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }
 
        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }
 
        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }
 
        public bool IsMutable
        {
            get { return false; }
        }
 
        public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
        {
            object obj = NHibernateUtil.BinaryBlob.NullSafeGet(rs, names[0]);
            if (obj == null) return null;
            var bytes = obj as byte[];
            using (var str = new MemoryStream(bytes))
            {
                return new Bitmap(str); 
            }
        }
 
        public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
        {
            var parameter = (IDataParameter)cmd.Parameters[index];
            if (value == null)
                parameter.Value = DBNull.Value;
            else
            {
                using (var str = new MemoryStream())
                {
                    var bmp =new Bitmap((Bitmap)value);
                    bmp.Save(str, ImageFormat.Png);
                    parameter.Value = str.ToArray();
                }
            }
        }
 
        public object Replace(object original, object target, object owner)
        {
            return original;
        }
 
        public Type ReturnedType
        {
            //the .Net type that this maps to
            get { return typeof(Bitmap); }
        }
 
        public NHibernate.SqlTypes.SqlType[] SqlTypes
        {
            //the sql type that this maps to
            get { return new NHibernate.SqlTypes.SqlType[] { NHibernateUtil.BinaryBlob.SqlType }; }
        }
 
        #endregion
    }
}
