// NativeAssemblyScratchpad.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "xmmintrin.h"
#include "immintrin.h"
#include "Timer.h"

using namespace std;

void GetRandomFloats(float* input, int size) {
	for (auto i = 0; i < size; i++) {
		input[i] = (rand() % 10) / 10.0;
	}
}
void GetRandomFloats(unsigned short* input, int size) {
	for (auto i = 0; i < size; i++) {
		input[i] = rand() % 10;
	}
}

bool simd256Available = false;
int gi;
unsigned long long PerformSimdArrayAdditionUsing128Bits(unsigned short* lhs, unsigned short* rhs, int setSize);
unsigned long long PerformSimdArrayAdditionUsing256Bits(unsigned short* lhs, unsigned short* rhs, int setSize);
unsigned long long PerformNaiveArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize);

int main() {
	auto testSetSize = 7680 * 4320;
	auto lhs = new unsigned short[testSetSize];
	auto rhs = new unsigned short[testSetSize];
	cout << "Generating test data... " << flush;
	GetRandomFloats(lhs, testSetSize);
	GetRandomFloats(rhs, testSetSize);
	cout << "done." << endl;

	auto hwAccTime128 = PerformSimdArrayAdditionUsing128Bits(lhs, rhs, testSetSize);
	auto naiveTime = PerformNaiveArrayAddition(lhs, rhs, testSetSize);

	if (simd256Available) {
		auto hwAccTime256 = PerformSimdArrayAdditionUsing256Bits(lhs, rhs, testSetSize);
		cout << "Speed up due to 256 hardware acceleration: " << (naiveTime / (double)hwAccTime256) * 100.0 << "%" << endl;
	}

	cout << "Speed up due to 128 hardware acceleration: " << (naiveTime / (double)hwAccTime128) * 100.0 << "%" << endl;
	cout << "Press any key to exit.";
	cin.ignore();

    return 0;
}

unsigned long long PerformSimdArrayAdditionUsing128Bits(unsigned short* lhs, unsigned short* rhs, int setSize) {
	__m128i a;
	__m128i b;
	auto result = new unsigned short[setSize];
	auto lhsEnd = lhs + setSize;
	auto rhsEnd = rhs + setSize;

	cout << "Performing hardware accelerated addition... " << flush;
	Timer::Start();
	for (auto i = 0; i < setSize; i += 8) {
		if (lhs >= lhsEnd) goto outofbounds;
		if (rhs >= rhsEnd) goto outofbounds;
		memcpy(a.m128i_u16, lhs + i, 16);
		if (lhs + 8 >= lhsEnd) goto outofbounds;
		if (rhs + 8 >= rhsEnd) goto outofbounds;
		memcpy(b.m128i_u16, rhs + i, 16);
		memcpy(result + i, _mm_adds_epi16(a, b).m128i_u16, 16);
	}
	Timer::Stop();
	auto elapsed = Timer::GetLapTimeMs();
	Timer::Reset();
	cout << endl << "HW accelerated addition complete in " << elapsed << "ms." << endl;

	for (auto i = 0; i < setSize; i++) {
		if (result[i] != lhs[i] + rhs[i]) {
			cout << "HW accelerated addition error at index" << i << endl;
		}
	}

	return elapsed;

outofbounds:
	cout << "Something was out of bounds, ending" << endl;
	return 0;
}

unsigned long long PerformSimdArrayAdditionUsing256Bits(unsigned short* lhs, unsigned short* rhs, int setSize) {
	__m256i a;
	__m256i b;
	auto result = new unsigned short[setSize];
	auto lhsEnd = lhs + setSize;
	auto rhsEnd = rhs + setSize;

	cout << "Performing hardware accelerated addition... " << flush;
	Timer::Start();
	for (auto i = 0; i < setSize; i += 16) {
		if (lhs >= lhsEnd) goto outofbounds;
		if (rhs >= rhsEnd) goto outofbounds;
		memcpy(a.m256i_u16, lhs + i, 32);
		if (lhs + 16 >= lhsEnd) goto outofbounds;
		if (rhs + 16 >= rhsEnd) goto outofbounds;
		memcpy(b.m256i_u16, rhs + i, 32);
		memcpy(result + i, _mm256_add_epi16(a, b).m256i_u16, 32);
	}
	Timer::Stop();
	auto elapsed = Timer::GetLapTimeMs();
	Timer::Reset();
	cout << endl << "HW accelerated addition complete in " << elapsed << "ms." << endl;

	for (auto i = 0; i < setSize; i++) {
		if (result[i] != lhs[i] + rhs[i]) {
			cout << "HW accelerated addition error at index" << i << endl;
		}
	}

	return elapsed;

outofbounds:
	cout << "Something was out of bounds, ending" << endl;
	return 0;
}

unsigned long long PerformNaiveArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize) {
	auto result = new unsigned short[setSize];

	cout << "Performing naive addition... " << flush;
	Timer::Start();
	for (gi = 0; gi < setSize; gi++) {	// gi declared at global scope to prevent unwanted compiler optimizations
		result[gi] = lhs[gi] + rhs[gi];
	}
	Timer::Stop();
	auto elapsed = Timer::GetLapTimeMs();
	Timer::Reset();
	cout << endl << "Naive addition complete in " << elapsed << "ms." << endl;

	for (auto i = 0; i < setSize; i++) {
		if (result[i] != lhs[i] + rhs[i]) {
			cout << "Naive addition error at index" << i << endl;
		}
	}
	return elapsed;
}