using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Email
    {
        public int Index;
        public string From;
        public string To;

        public string Subject;
        public string MessageBody;

        public int ForwardScore;
        public int ReportScore;
    }
}

