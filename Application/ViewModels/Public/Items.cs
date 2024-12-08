﻿namespace Application.ViewModel.Public
{
    public class QueryItems
    {
        public QueryItems(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }
}