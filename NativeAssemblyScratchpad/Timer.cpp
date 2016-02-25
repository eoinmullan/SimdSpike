#include "stdafx.h"
#include "Timer.h"
#include <Windows.h>
#include <string>

unsigned long long Timer::timerStart = 0;
long long Timer::cpuFrequency = 1;
unsigned long long Timer::timeAccumulatedBeforeStart = 0;
bool Timer::running = false;

void Timer::Start()
{
	LARGE_INTEGER freq;
	if (!QueryPerformanceFrequency(&freq)) {
		throw std::string("Failed to query performance counter");
	}
	cpuFrequency = freq.QuadPart;

	if (!running) {
		timerStart = currentClockTicks();
	}
	running = true;
}

void Timer::Stop()
{
	timeAccumulatedBeforeStart += currentClockTicks() - timerStart;
	running = false;
}

void Timer::Reset()
{
	timeAccumulatedBeforeStart = 0;
	timerStart = currentClockTicks();
}

unsigned long long Timer::GetLapTimeMs()
{
	if (running) {
		return ((currentClockTicks() - timerStart + timeAccumulatedBeforeStart) * 1000) / cpuFrequency;
	}
	else {
		return ((timeAccumulatedBeforeStart)* 1000) / cpuFrequency;
	}
}

double Timer::GetLapTimeS()
{
	if (running) {
		return static_cast<double>((currentClockTicks() - timerStart + timeAccumulatedBeforeStart) / static_cast<double>(cpuFrequency));
	}
	else {
		return static_cast<double>((timeAccumulatedBeforeStart) / static_cast<double>(cpuFrequency));
	}
}

inline unsigned long long Timer::currentClockTicks()
{
	LARGE_INTEGER currentTime;
	if (!QueryPerformanceCounter(&currentTime)) {
		throw std::string("Failed to query performance counter");
	}
	return currentTime.QuadPart;
}