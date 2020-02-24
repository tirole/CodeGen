#include <stdlib.h>
#include <iostream>
#include "../../CommaSeparatedResource\Output/SamplerDescriptor.h"
#include "../../CommaSeparatedResource\Output/TextureDescriptor.h"

#include "../../JsonResource/Outputs/my_RenderTargetDescriptor.h"
#include "../../JsonResource/Outputs/my_Enum.h"

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
	}

	{
		my::detail::RendeerTargetDescriptor desc = {};
		auto pDesc = &desc;
		const int32_t colors[4] = { 1, 2, 3, 4 };
		my::detail::SetRendeerTargetDescriptorClearColors( pDesc, colors );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset( pDesc, 5 );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset2( pDesc, 6 );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset3( pDesc, my::detail::CompressionMode::Block );
		int32_t outColors[4] = {};
		my::detail::GetRendeerTargetDescriptorClearColors( outColors, pDesc );
		for(int i = 0; i < 4; ++i)
		{
			TEST_EQ(outColors[i], colors[i]);
		}
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset( pDesc ), 5 - 1);
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset2( pDesc ), 6 );
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset3( pDesc ), my::detail::CompressionMode::Block );
	}

	return 0;
}
