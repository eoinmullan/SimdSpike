#pragma once

class Timer
{
public:
	static void Start();
	static void Stop();
	static void Reset();
	static unsigned long long GetLapTimeMs();
	static double GetLapTimeS();

private:
	static unsigned long long currentClockTicks();

	static bool running;
	static unsigned long long timerStart;
	static long long cpuFrequency;
	static unsigned long long timeAccumulatedBeforeStart;
};