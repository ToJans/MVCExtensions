using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcExtensions.Model
{
    public static class StringExtensions
    {
        public static string IsNotEmpty(this string  me)
        {
            return string.IsNullOrEmpty(me)?"Value can not be empty":null;
        }

        public static string IsLengthSmallerThen( this string me,int length)
        {
            return me==null||me.Length<=length?null:"Value is too long (max "+length.ToString()+ " characters";
        }

        public static string IsRegexMatch( this string me,string regex,string msg)
        {
            return me==null||Regex.IsMatch(me,regex)?null:msg;
        }
    }

    public abstract class MyText
    {

        public MyText()
        {
            _values = new string[] { null};
        }

        protected virtual int GetValueIndex
        {
            get { return 0; }
        }

        protected string[] _values;
        public virtual string Value
        {
            get { return DoGetValue(); }
            set { DoSetValue(value); }
        }

        public override bool Equals(object obj)
        {
            if (Value == null) return obj == null;
            return Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            if (Value == null) return Value;
            return Value.ToString();
        }

        public static implicit operator string(MyText s)
        {
            if (s == null) return null;
            return s.Value;
        }

        protected static T Assign<T>(string s) where T : MyText, new()
        {
            var res = new T();
            res.Value = s;
            return res;
        }

        public virtual void DoSetValue(string v)
        {
            _values[GetValueIndex] = v;
        }

        public virtual string DoGetValue()
        {
            return _values[GetValueIndex];
        }
    }

    public abstract class MyValidatedText : MyText 
    {
        protected List<Func<string,string>> ValidationChecks = new List<Func<string,string>>();
        public MyValidatedText(params Func<string,string>[] ValidationChecks)
        {
            this.ValidationChecks.AddRange(ValidationChecks);
        }

        public string Validate(string value,bool throwex)
        {
            var val= ValidationChecks.Select(x => x(value)).FirstOrDefault();
            if (val != null && throwex)
                throw new ArgumentException(val);
            else
                return val;
        }

        public override void DoSetValue(string value)
        {
            Validate(value,true);
            base.DoSetValue(value);
        }

    }

    public abstract class MyValidatedXlatText : MyValidatedText
    {
        protected MyValidatedXlatText(params Func<string, string>[] ValidationChecks) : base(ValidationChecks)
        {
            _values = new string[] { null, null, null, null,null };
        }

        protected override int GetValueIndex
        {
            get
            {
                var x = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                switch (x)
                {
                    case "fr": return 1; break;
                    case "en": return 2; break;
                    case "de": return 3; break;
                    case "pl": return 4; break;
                    default: return 0;
                }
            }
        }

        public override string DoGetValue()
        {
            var x = _values[GetValueIndex];
            return string.IsNullOrEmpty(x)?"[NL]"+NL:x;
        }

        public override void DoSetValue(string value)
        {
            Validate(value, true);
            _values[GetValueIndex] = value;
            if (!string.IsNullOrEmpty(value) && NL == null)
                NL = value;
        }


        public virtual string NL { get { return _values[0]; } set { _values[0] = value; } }
        public virtual string FR { get { return _values[1]; } set { _values[1] = value; } }
        public virtual string EN { get { return _values[2]; } set { _values[2] = value; } }
        public virtual string DE { get { return _values[3]; } set { _values[3] = value; } }
        public virtual string PL { get { return _values[4]; } set { _values[4] = value; } }
    }

    public class ShortText : MyValidatedText 
    {
        public ShortText() : base(x => x.IsLengthSmallerThen(16)) { }
        public static implicit operator string(ShortText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator ShortText(string value)
        {
            return new ShortText() { Value = value };
        }
    }

    public class NonEmptyShortText : MyValidatedText
    {
        public NonEmptyShortText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(16)) { }
        public static implicit operator string(NonEmptyShortText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator NonEmptyShortText(string value)
        {
            return new NonEmptyShortText() { Value = value };
        }
    }

    public class NormalText : MyValidatedText
    {
        public NormalText() : base(x => x.IsLengthSmallerThen(256)) { }
        public static implicit operator string(NormalText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator NormalText(string value)
        {
            return new NormalText() { Value = value };
        }
    }

    public class NonEmptyNormalText : MyValidatedText
    {
        public NonEmptyNormalText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(256)) { }
        public static implicit operator string(NonEmptyNormalText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator NonEmptyNormalText(string value)
        {
            return new NonEmptyNormalText() { Value = value };
        }
    }

    public class LongText : MyValidatedText
    {
        public LongText() : base(x => x.IsLengthSmallerThen(1024)) { }
        public static implicit operator LongText(string value)
        {
            return new LongText() { Value = value };
        }
    }

    public class NonEmptyLongText : MyValidatedText
    {
        public NonEmptyLongText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(1024)) { }
        public static implicit operator NonEmptyLongText(string value)
        {
            return new NonEmptyLongText() { Value = value };
        }
    }

    public class MemoText : MyValidatedText
    {
        public MemoText() : base() { }
        public static implicit operator MemoText(string value)
        {
            return new MemoText() { Value = value};
        }
    }

    public class NonEmptyMemoText : MyValidatedText
    {
        public NonEmptyMemoText() : base(x => x.IsNotEmpty()) { }
        public static implicit operator string(NonEmptyMemoText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator NonEmptyMemoText(string value)
        {
            return new NonEmptyMemoText() { Value = value };
        }
    }

    public class EmailText : MyValidatedText
    {
        public EmailText():base(x=>x.IsRegexMatch(@"[a-z0-9._%-]+@[a-z0-9.-]+\.[A-Za-z]{2,4}","Invalid email address")) {}
        public override void  DoSetValue(string value)
        {
            if (value!=null) value = value.ToLowerInvariant();
 	        base.DoSetValue(value);
        }

        public static implicit operator string(EmailText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator EmailText(string value)
        {
            return new EmailText() { Value = value };
        }

    }

    public class XShortText : MyValidatedXlatText
    {
        public XShortText() : base(x => x.IsLengthSmallerThen(16)) { }

    }

    public class XNonEmptyShortText : MyValidatedXlatText
    {
        public XNonEmptyShortText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(16)) { }

        public static implicit operator string(XNonEmptyShortText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator XNonEmptyShortText(string value)
        {
            return new XNonEmptyShortText() { Value = value };
        }

    }

    public class XNormalText : MyValidatedXlatText
    {
        public XNormalText() : base(x => x.IsLengthSmallerThen(256)) { }
    }

    public class XNonEmptyNormalText : MyValidatedXlatText
    {
        public XNonEmptyNormalText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(256)) { }
        public static implicit operator string(XNonEmptyNormalText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator XNonEmptyNormalText(string value)
        {
            return new XNonEmptyNormalText() { Value = value };
        }

    }

    public class XLongText : MyValidatedXlatText
    {
        public XLongText() : base(x => x.IsLengthSmallerThen(1024)) { }
        public static implicit operator string(XLongText s)
        {
            return s == null ? null : s.Value;
        }
        public static implicit operator XLongText(string value)
        {
            return new XLongText() { Value = value };
        }
    }

    public class XNonEmptyLongText : MyValidatedXlatText
    {
        public XNonEmptyLongText() : base(x => x.IsNotEmpty(), x => x.IsLengthSmallerThen(1024)) { }
    }

    public class XMemoText : MyValidatedXlatText
    {
        public XMemoText() : base() { }
    }

    public class XNonEmptyMemoText : MyValidatedXlatText
    {
        public XNonEmptyMemoText() : base(x => x.IsNotEmpty()) { }
    }
}
