﻿using System;


namespace Acr.Ef {

    public class Pager {
        
        public bool UsePages { get; set; }
        public int MaxResults { get; set; }
        public int Start { get; set; }
        public string[] Sorts { get; set; }
    }
}
