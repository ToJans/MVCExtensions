using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcExtensions.Model
{
    public abstract class MyText
    {
        private string _value;
        public virtual string Value 
        {
            get { return _value;}
            set
            {
                if (string.IsNullOrEmpty(value) && !CanBeEmpty)
                    throw new ArgumentNullException("String can not be empty");
                else if (value != null && value.Length > Length)
                    throw new ArgumentOutOfRangeException("String is too long");
                else if (Regex != null && !Regex.IsMatch(value))
                    throw new ArgumentOutOfRangeException("String does not match regular expression");
                else
                    _value = value;
            }
        }

        public virtual int Length { get { return int.MaxValue; } }
        public virtual bool CanBeEmpty { get { return true; } }
        public virtual Regex Regex { get { return null; } }

        public MyText()
        {
        }

        public MyText(string s)
        {
            this.Value = s;
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

        protected static T Assign<T>(string s) where T:MyText,new()
        {
            var res = new T();
            res.Value = s;
            return res;
        }
    }

    public class ShortText : MyText 
    {
        public override int Length {get { return 32; }}
        public ShortText(){ }
        public ShortText(string s) : base(s) { }
        public static implicit operator ShortText(string s) { return new ShortText(s); }
    }

    public class NonEmptyShortText : ShortText
    {
        public override bool CanBeEmpty { get { return false; } }
        public NonEmptyShortText() { }
        public NonEmptyShortText(string s) : base(s) { }
        public static implicit operator NonEmptyShortText(string s) { return new NonEmptyShortText(s); }
    }

    public class NormalText : MyText
    {
        public override int Length { get { return 256; } }
        public NormalText(){ }
        public NormalText(string s) : base(s) { }
        public static implicit operator NormalText(string s) { return new NormalText(s); }
    }

    public class NonEmptyNormalText : NormalText
    {
        public override bool CanBeEmpty { get { return false; } }
        public NonEmptyNormalText(){ }
        public NonEmptyNormalText(string s) : base(s) { }
        public static implicit operator NonEmptyNormalText(string s) { return new NonEmptyNormalText(s); }
    }

    public class LongText : MyText
    {
        public override int Length { get { return 1024; } }
        public LongText(){ }
        public LongText(string s) : base(s) { }
        public static implicit operator LongText(string s) { return new LongText(s); }
    }

    public class NonEmptyLongText : LongText
    {
        public override bool CanBeEmpty { get { return false; } }
        public NonEmptyLongText(){ }
        public NonEmptyLongText(string s) : base(s) { }
        public static implicit operator NonEmptyLongText(string s) { return new NonEmptyLongText(s); }
    }

    public class MemoText : MyText
    {
        public override int Length { get { return int.MaxValue; } }
        public MemoText(){ }
        public MemoText(string s) : base(s) { }
        public static implicit operator MemoText(string s) { return new MemoText(s); }
    }

    public class NonEmptyMemoText : MemoText
    {
        public override bool CanBeEmpty { get { return false; } }
        public NonEmptyMemoText(){ }
        public NonEmptyMemoText(string s) : base(s) { }
        public static implicit operator NonEmptyMemoText(string s) { return new NonEmptyMemoText(s); }
    }
}
