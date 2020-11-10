using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class BrokenBusinessRule
    {
        public string Property { get; set; }

        public string Rule { get; set; }


        public BrokenBusinessRule(string property, string rule)
        {
            Property = property;
            Rule = rule;
        }

        public BrokenBusinessRule()
        {
        }

    }
}
