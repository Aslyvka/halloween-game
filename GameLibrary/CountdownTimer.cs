using System;
using Windows.UI.Xaml;
//Name: Anastasiia Slyvka
//Date: October 5, 2023
//Project: Lab 1 - Game
public class CountdownTimer
{
	private DispatcherTimer timer;
	private int seconds;

	public event Action<int> TimerTick; 
	public event Action TimeElapsed;
	//Constructor that indicates the duration of the times
	public CountdownTimer(int seconds)
	{
		this.seconds = seconds;
		this.timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
		this.timer.Tick += OnTick;
	}
	//Ticking every second backwards
	private void OnTick(object sender, object e)
	{
		if (seconds > 0)
		{
			seconds--;
			TimerTick?.Invoke(seconds);
		}
		//timer stops after reaching 0
		else
		{
			timer.Stop();
			TimeElapsed?.Invoke();
		}
	}
	//Timer is set to 30 seconds as default
	public void Reset(int newSeconds = 30)
	{
		this.seconds = newSeconds;
		timer.Stop();
		TimerTick?.Invoke(this.seconds);
	}
	//Methods to start and stop the timer
	public void Start() => timer.Start();
	public void Stop() => timer.Stop();
}
