using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace FinancialReview
{
    public class FinancialReviewItem : DynamicObject, INotifyPropertyChanged
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        // Standard properties
        public bool IsSelected { get; set; }
        public string Status { get; set; }
        public string RequestName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Client { get; set; }
        public int DaysRemaining { get; set; }
        public int DaysOutstanding { get; set; }

        // New property for document count to be added to RequestName
        public int DocumentCount { get; set; }

        // Indexer to access dynamic properties
        public object this[string propertyName]
        {
            get
            {
                if (_properties.TryGetValue(propertyName, out object value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _properties[propertyName] = value;
                OnPropertyChanged(propertyName);
            }
        }

        // Method to get dynamic property names
        public IEnumerable<string> GetDynamicPropertyNames()
        {
            return _properties.Keys;
        }

        // **Add the ContainsProperty method**
        public bool ContainsProperty(string propertyName)
        {
            return _properties.ContainsKey(propertyName);
        }

        // **Add the RemoveProperty method**
        public void RemoveProperty(string propertyName)
        {
            if (_properties.Remove(propertyName))
            {
                OnPropertyChanged(propertyName);
            }
        }

        // Dynamic properties handling
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            OnPropertyChanged(binder.Name);
            return true;
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
