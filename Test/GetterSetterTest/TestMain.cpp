#include <stdlib.h>
#include <iostream>
#include "DummySdkDefinitions.h"
#include "../../CommaSeparatedResource\Output/SamplerDescriptor.h"
#include "../../CommaSeparatedResource\Output/TextureDescriptor.h"
#include "../../JsonResource/Outputs/my_SimpleStruct.h"
#include "../../JsonResource/Outputs/my_Enum.h"
#include "../../JsonResource/Outputs/my_RenderTargetDescriptor.h"
#include "../../JsonResource/Outputs/my_GpuCommand.h"

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
		my::detail::SimpleUnion simpleUnion;
		simpleUnion.intVal = 0xdead;
		my::detail::SetRendeerTargetDescriptorClearColors( pDesc, colors );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset( pDesc, 5 );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset2( pDesc, 6 );
		my::detail::SetRendeerTargetDescriptorSrcBufOsset3( pDesc, my::detail::CompressionMode::Block );
		my::detail::SetRendeerTargetDescriptorFormatDescriptorUnion( pDesc, &simpleUnion);
		uint64_t pointer = 0xdeLL << 50;
		pointer |= 0xdead;
		my::detail::SetRendeerTargetDescriptorPointer( pDesc, pointer);
		int32_t outColors[4] = {};
		my::detail::GetRendeerTargetDescriptorClearColors( outColors, pDesc );
		for(int i = 0; i < 4; ++i)
		{
			TEST_EQ(outColors[i], colors[i]);
		}
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset( pDesc ), 5 - 1);
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset2( pDesc ), 6 );
		TEST_EQ( my::detail::GetRendeerTargetDescriptorSrcBufOsset3( pDesc ), my::detail::CompressionMode::Block );

		// modifiler ‚Å 2 ”{‚³‚ê‚Ä‚¢‚é‚Ì‚Å 2 * pointer
		TEST_EQ( my::detail::GetRendeerTargetDescriptorPointer( pDesc ), (2 * pointer) & (~(-1LL << 58)) );
		my::detail::SimpleUnion out;
		my::detail::GetRendeerTargetDescriptorFormatDescriptorUnion(&out, pDesc );
		TEST_EQ( out.intVal, simpleUnion.intVal );
	}

	{
		const my::detail::FlagDescriptor flagDesc =
		{
			0xdead
		};
		const int index = 4;
		CommandData cmd = my::detail::Move( flagDesc, index );
		CommandData cmdGoldenResult =
		{
			{ (static_cast<uint64_t>(my::detail::Opcode::Move) << 56) | ( static_cast< uint64_t >( index ) << 48 ) | 0xdead }
		};
		TEST_EQ( cmd.data, cmdGoldenResult.data );
	}

	return 0;
}
