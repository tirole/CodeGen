#pragma once
#include <inttypes.h>

#define SDK_MIN_MAX(var, min, max)
#define SDK_LESS(var, val)
#define SDK_NOT_NULL(var)
#define SDK_LESS_EQUAL(var, val)
#define SDK_NOT_EQUAL(var, val)

typedef uint64_t GpuAddress;
enum class FilterMode
{
	Point,
	Linear,
};