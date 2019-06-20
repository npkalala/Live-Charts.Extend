using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Live_Charts.Demo
{
    public abstract class PropertyObserver<TModel> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private PropertyValueHash<TModel> ValueHash = new PropertyValueHash<TModel>();

        /// <summary>
        /// 觀察者Value Setter
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="newValue">value</param>
        /// <param name="callerName">caller name</param>
        /// <returns></returns>
        public bool SetValue<TValue>(TValue newValue, [CallerMemberName] string callerName = "")
        {
            if (!EqualityComparer<TValue>.Default.Equals((TValue)ValueHash[callerName], newValue))
            {
                ValueHash[callerName] = newValue;
                this.PropertyOnChange(callerName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 觀察者Value Getter
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="callerName">caller name</param>
        /// <returns></returns>
        public TValue GetValue<TValue>([CallerMemberName] string callerName = "")
        {
            return (TValue)ValueHash[callerName];
        }

        /// <summary>
        /// 觀察者Value Getter
        /// </summary>
        /// <param name="callerName">caller name</param>
        /// <returns></returns>
        public object GetValue([CallerMemberName] string callerName = "")
        {
            return ValueHash[callerName];
        }

        /// <summary>
        /// 觀察者Property Value Change trigger
        /// </summary>
        /// <param name="propertyName"></param>
        private void PropertyOnChange(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }

    class PropertyValueHash<TModel> : Dictionary<string, object>
    {
        public new object this[string propName]
        {
            get
            {
                CheckContainKey(propName);
                return base[propName];
            }

            set
            {
                CheckContainKey(propName);
                base[propName] = value;
            }
        }

        private void CheckContainKey(string propName)
        {
            if (!this.ContainsKey(propName))
            {
                Type propType = typeof(TModel).GetProperty(propName).PropertyType;
                this.Add(propName, propType.IsValueType ? Activator.CreateInstance(propType) : null);
            }

        }
    }

    // Version 2
    public class PropertyObserver : INotifyPropertyChanged
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetValue<T>([CallerMemberName] string name = null)
        {
            //Debug.Assert(name != null, "name != null");
            object value = null;
            if (_properties.TryGetValue(name, out value))
                return value == null ? default(T) : (T)value;
            return default(T);
        }

        /// <summary>
        /// Sets the value of a property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <remarks>Use this overload when implicitly naming the property</remarks>
        protected void SetValue<T>(T value, [CallerMemberName] string name = null)
        {
            //Debug.Assert(name != null, "name != null");
            if (Equals(value, GetValue<T>(name)))
                return;
            _properties[name] = value;
            OnPropertyChanged(name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
