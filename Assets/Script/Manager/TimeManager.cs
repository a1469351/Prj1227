using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ns
{
	public delegate void TimeTaskDelegate();
	public class Timer
    {
		private float _delay;
		private float _oriDelayTime;
		private bool _repeat;
		public bool isStop;
		private TimeTaskDelegate _timerCallBack;
		public float DelayTime
        {
			get { return _delay; }
			set { _delay = value; }
        }
		public float OriDelayTime
		{
			get { return _oriDelayTime; }
			set { _oriDelayTime = value; }
		}
		public bool Repeat
        {
			get { return _repeat; }
			set { _repeat = value; }
        }
		public TimeTaskDelegate TimerCallBack
        {
			get { return _timerCallBack; }
			set { _timerCallBack = value; }
        }
		public Timer(float timeDelay, TimeTaskDelegate callback, bool repeat = false)
        {
			_delay = timeDelay;
			_oriDelayTime = timeDelay;
			_timerCallBack = callback;
			_repeat = repeat;
			isStop = false;
        }

		public void Action()
        {
			if (_timerCallBack != null) _timerCallBack();
        }

		public void Stop()
        {
			isStop = true;
        }

		public void Resume()
        {
			isStop = false;
        }

		public void Reset()
        {
			_delay = _oriDelayTime;
        }
    }
	public class TimeManager : SingletonBase<TimeManager>
	{
		private List<Timer> timerList = new List<Timer>();

		public Timer AddTimer(float delay, TimeTaskDelegate callback, bool repeat = false)
        {
			Timer t = new Timer(delay, callback, repeat);
			timerList.Add(t);
			return t;
        }

		public bool RemoveTimer(TimeTaskDelegate callback)
        {
			if (timerList.Count == 0 || callback == null) return false;
			foreach (Timer t in timerList)
            {
				if (t.TimerCallBack == callback)
                {
					return timerList.Remove(t);
                }
            }
			return false;
        }

		public bool RemoveTimer(Timer t)
		{
			if (timerList.Count == 0) return false;
			return timerList.Remove(t);
		}

		private void Update()
        {
			Tick();
		}

        private void Tick()
        {
			List<Timer> rmvList = new List<Timer>();
			foreach (Timer t in timerList)
            {
				if (t.isStop) continue;
				t.DelayTime -= Time.deltaTime;
				if (t.DelayTime <= 0)
                {
					t.Action();
					if (t.Repeat)
                    {
						t.DelayTime = t.OriDelayTime;
                    }
                    else
                    {
						rmvList.Add(t);
					}
                }
            }
			foreach (Timer t in rmvList)
            {
				timerList.Remove(t);
            }
        }

		public void StopAllTimer()
        {
			foreach (Timer t in timerList)
            {
				t.Stop();
            }
        }

		public void ResumeAllTimer()
		{
			foreach (Timer t in timerList)
			{
				t.Resume();
			}
		}
	}
}