using System;
namespace Marmalade
{
	public class Log
	{
		int size;
		int curSize;
		Queue<string> logs = new Queue<string>();

		public Log(int s)
		{
			curSize = 0;
			size = s;
		}

		public void enque(string msg)
        {
			if (curSize >= size)
            {
				logs.Dequeue();
				logs.Enqueue(msg);
            } // fine i guess, but learn about circle queue, that is what this needs, only show first and remove first(last one shown)
			else
			{
				logs.Enqueue(msg);
			}
        }
	}
}

