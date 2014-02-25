using System;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace Acr.Collections {

    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged {
        private const string CountString = "Count";
        private const string IndexerName = "Item[]";
        private const string KeysName = "Keys";
        private const string ValuesName = "Values";

        protected IDictionary<TKey, TValue> Dictionary { get; private set; }

        #region ctor

        public ObservableDictionary() {
            this.Dictionary = new Dictionary<TKey, TValue>();
        }


        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) {
            this.Dictionary = new Dictionary<TKey, TValue>(dictionary);
        }


        public ObservableDictionary(IEqualityComparer<TKey> comparer) {
            this.Dictionary = new Dictionary<TKey, TValue>(comparer);
        }


        public ObservableDictionary(int capacity) {
            this.Dictionary = new Dictionary<TKey, TValue>(capacity);
        }


        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {
            this.Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }


        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) {
            this.Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value) {
            this.Insert(key, value, true);
        }


        public bool ContainsKey(TKey key) {
            return this.Dictionary.ContainsKey(key);
        }


        public ICollection<TKey> Keys {
            get { return this.Dictionary.Keys; }
        }


        public bool Remove(TKey key) {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue value;
            this.Dictionary.TryGetValue(key, out value);
            var removed = this.Dictionary.Remove(key);
            if (removed) {
                this.OnCollectionChanged();
            }
            return removed;
        }


        public bool TryGetValue(TKey key, out TValue value) {
            return this.Dictionary.TryGetValue(key, out value);
        }


        public ICollection<TValue> Values {
            get { return this.Dictionary.Values; }
        }


        public TValue this[TKey key] {
            get {
                return this.Dictionary[key];
            }
            set {
                this.Insert(key, value, false);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item) {
            this.Insert(item.Key, item.Value, true);
        }

        public void Clear() {
            if (Dictionary.Count == 0)
                return;

            this.Dictionary.Clear();
            this.OnCollectionChanged();
        }


        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return this.Dictionary.Contains(item);
        }


        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            this.Dictionary.CopyTo(array, arrayIndex);
        }


        public int Count {
            get { return this.Dictionary.Count; }
        }


        public bool IsReadOnly {
            get { return this.Dictionary.IsReadOnly; }
        }


        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return this.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return this.Dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)this.Dictionary).GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public void AddRange(IDictionary<TKey, TValue> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            if (items.Count == 0)
                return;

            if (this.Dictionary.Count == 0) {
                this.Dictionary = new Dictionary<TKey, TValue>(items);
            }
            else {
                if (items.Keys.Any(this.Dictionary.ContainsKey))
                    throw new ArgumentException("An item with the same key has already been added.");

                foreach (var item in items) {
                    this.Dictionary.Add(item);
                }
            }
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
        }


        protected virtual void OnPropertyChanged(string propertyName) {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Insert(TKey key, TValue value, bool add) {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue item;
            if (this.Dictionary.TryGetValue(key, out item)) {
                if (add)
                    throw new ArgumentException("An item with the same key has already been added.");

                if (Equals(item, value))
                    return;

                this.Dictionary[key] = value;
                var index = this.Dictionary.Keys.ToList().IndexOf(key);

                this.OnCollectionChanged(
                    NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<TKey, TValue>(key, value),
                    new KeyValuePair<TKey, TValue>(key, item),
                    index
                );
            }
            else {
                this.Dictionary[key] = value;
                var index = this.Dictionary.Keys.ToList().IndexOf(key);
                this.OnCollectionChanged(
                    NotifyCollectionChangedAction.Add,
                    new KeyValuePair<TKey, TValue>(key, value),
                    index
                );
            }
        }


        private void OnPropertyChanged() {
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
            this.OnPropertyChanged(KeysName);
            this.OnPropertyChanged(ValuesName);
        }


        private void OnCollectionChanged() {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem, int index) {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem, int index) {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems) {
            this.OnPropertyChanged();
            if (this.CollectionChanged != null) 
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems, -1));
        }

        #endregion
    }
}
