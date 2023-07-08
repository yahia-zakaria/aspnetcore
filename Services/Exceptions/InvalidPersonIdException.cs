using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
	public class InvalidPersonIdException : ArgumentException 
	{
		public InvalidPersonIdException() : base() { }
		public InvalidPersonIdException(string message) : base(message) { }
		public InvalidPersonIdException(string message, Exception? innerException) : base(message, innerException) { }
	}
}
