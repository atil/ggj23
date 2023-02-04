using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class Email
	{
		public int index;
		public string from;
		public string to;

		public string subject;
		public string messageBody;

		public EmailResult expectedResult;
	}
}

