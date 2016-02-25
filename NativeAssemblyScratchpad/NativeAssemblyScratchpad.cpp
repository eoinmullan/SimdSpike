// NativeAssemblyScratchpad.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "xmmintrin.h"

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
int gi;
void PerformSimdArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize);
void PerformNaiveArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize);

int main() {
	auto testSetSize = 12;
	auto lhs = new unsigned short[testSetSize];
	auto rhs = new unsigned short[testSetSize];
	GetRandomFloats(lhs, testSetSize);
	GetRandomFloats(rhs, testSetSize);

	PerformSimdArrayAddition(lhs, rhs, testSetSize);
	PerformNaiveArrayAddition(lhs, rhs, testSetSize);

	cin.ignore();

    return 0;
}

 void PerformSimdArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize) {
	__m128i a;
	__m128i b;
	auto result = new unsigned short[setSize];
	auto lhsEnd = lhs + setSize;
	auto rhsEnd = rhs + setSize;
	for (auto i = 0; i < setSize; i += 8) {
		if (lhs >= lhsEnd) goto outofbounds;
		if (rhs >= rhsEnd) goto outofbounds;
		memcpy(a.m128i_u16, lhs + i, 16);
		if (lhs + 8 >= lhsEnd) goto outofbounds;
		if (rhs + 8 >= rhsEnd) goto outofbounds;
		memcpy(b.m128i_u16, rhs + i, 16);
		memcpy(result + i, _mm_adds_epi16(a, b).m128i_u16, 16);
	}

	for (auto i = 0; i < setSize; i++) {
		if (result[i] != lhs[i] + rhs[i]) {
			cout << "HW accelerated addition error at index" << i << endl;
		}
		cout << lhs[i] << " + " << rhs[i] << " = " << result[i] << " " << (lhs[i] + rhs[i] == result[i] ? "(correct)" : "(wrong)") << endl;
	}

	return;

outofbounds:
	cout << "Something was out of bounds, ending" << endl;
	return;
}

void PerformNaiveArrayAddition(unsigned short* lhs, unsigned short* rhs, int setSize) {
	auto result = new unsigned short[setSize];

	for (gi = 0; gi < setSize; gi++) {	// gi declared at global scope to prevent unwanted compiler optimizations
		result[gi] = lhs[gi] + rhs[gi];
	}

	for (auto i = 0; i < setSize; i++) {
		if (result[i] != lhs[i] + rhs[i]) {
			cout << "HW accelerated addition error at index" << i << endl;
		}
		cout << lhs[i] << " + " << rhs[i] << " = " << result[i] << " " << (lhs[i] + rhs[i] == result[i] ? "(correct)" : "(wrong)") << endl;
	}
	return;
}