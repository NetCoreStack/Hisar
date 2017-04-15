using System;

namespace NetCoreStack.Hisar
{
    public class WebDecoratorNames
    {
        private readonly string _tagName;
        private int _hashcode;

        private static readonly WebDecoratorNames _link = new WebDecoratorNames("link");
        private static readonly WebDecoratorNames _script = new WebDecoratorNames("script");
        private static readonly WebDecoratorNames _meta = new WebDecoratorNames("meta");

        public static WebDecoratorNames Link
        {
            get
            {
                return _link;
            }
        }

        public static WebDecoratorNames Script
        {
            get
            {
                return _script;
            }
        }

        public static WebDecoratorNames Meta
        {
            get
            {
                return _meta;
            }
        }

        public string TagName
        {
            get { return _tagName; }
        }

        public WebDecoratorNames(string tagName)
        {
            _tagName = tagName;
        }

        public bool Equals(WebDecoratorNames other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(_tagName, other._tagName))
            {
                return true;
            }

            return string.Equals(_tagName, other._tagName, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WebDecoratorNames);
        }

        public override int GetHashCode()
        {
            if (_hashcode == 0)
            {
                _hashcode = _tagName.GetHashCode();
            }

            return _hashcode;
        }

        public override string ToString()
        {
            return _tagName.ToString();
        }

        public static bool operator ==(WebDecoratorNames left, WebDecoratorNames right)
        {
            if ((object)left == null)
            {
                return ((object)right == null);
            }
            else if ((object)right == null)
            {
                return ((object)left == null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(WebDecoratorNames left, WebDecoratorNames right)
        {
            return !(left == right);
        }
    }
}
