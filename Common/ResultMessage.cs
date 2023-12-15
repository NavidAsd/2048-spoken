
using System.Net;

namespace Shared
{
	public class ResultMessage<T>
	{
		public bool Success { set; get; }
		public string Message { set; get; }
		public T Enything { set; get; }
	}
	public class ResultMessage
	{
		public bool Success { set; get; }
		public string Message { set; get; }
	}
	public class ResultMessageApi<T>
	{
		public bool Success { set; get; }
		public string Message { set; get; }
        public HttpStatusCode StatusCode { set; get; }
        public T Enything { set; get; }
	}
	public class ResultMessageApi
    {
		public bool Success { set; get; }
		public string Message { set; get; }
		public HttpStatusCode StatusCode { set; get; }
	}
}
