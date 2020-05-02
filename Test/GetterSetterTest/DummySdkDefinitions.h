#pragma once
#include <cstdint>

#define SDK_MIN_MAX(var, min, max)
#define SDK_LESS(var, val)
#define SDK_NOT_NULL(var)
#define SDK_LESS_EQUAL(var, val)
#define SDK_NOT_EQUAL(var, val)
#define SDK_ALIGN(var, val)
#define SDK_EQUAL(var, val)

typedef uint64_t GpuAddress;
typedef uint64_t GpuCommand;
enum class FilterMode
{
	Point,
	Linear,
};

uint64_t Ceil( uint64_t val )
{
	return val;
}

struct CommandData
{
	uint64_t data;
	constexpr CommandData( uint64_t val ) : data(val) {}
	constexpr CommandData() : data() {}
};

class CommandBase : public CommandData
{
public:
	using CommandData::CommandData;
};
