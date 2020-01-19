#include <stdlib.h>
#include <iostream>
#include "../../CommaSeparatedResource\Output/SamplerDescriptor.h"
#include "../../CommaSeparatedResource\Output/TextureDescriptor.h"

#define TEST_EQ(val1, val2)\
{\
if( (val1) != (val2) )\
{\
	std::cout << "Failed: val1 != val2\n";\
	return 1;\
}\
}\

int main()
{
	SamplerDescriptor desc = {};
	auto pDesc = &desc;
	SetSamplerDescriptorConversionDescriptor( pDesc, 0xdead );
	SetSamplerDescriptorFilterMode( pDesc, FilterMode::Linear );
	SetSamplerDescriptorLodMax( pDesc, 4 );
	SetSamplerDescriptorLodMin( pDesc, 2 );
	SetSamplerDescriptorMaxAniso( pDesc, 15 );

	TEST_EQ( GetSamplerDescriptorConversionDescriptor( pDesc ), 0xdead );
	TEST_EQ( GetSamplerDescriptorFilterMode( pDesc ), FilterMode::Linear );
	TEST_EQ( GetSamplerDescriptorLodMax( pDesc ), 4 );
	TEST_EQ( GetSamplerDescriptorLodMin( pDesc ), 2 );
	TEST_EQ( GetSamplerDescriptorMaxAniso( pDesc ), 15 );

	return 0;
}
