using System;


namespace Acr.Ef {

    public class DataPage<T> where T : class {

        public int TotalCount { get; set; }
        public T[] Data { get; set; }
    }
}
