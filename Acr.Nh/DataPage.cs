using System;


namespace Acr.Nh {

    public class DataPage<T> where T : class {

        public int TotalCount { get; set; }
        public T[] Data { get; set; }
    }
}
