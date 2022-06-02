using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Road
{
    public class StraightChunkGeneration : IChunkGenerationStrategy
    {
        public Chunk Generate(Chunk chunkToFill)
        {
            return chunkToFill;
        }
    }
}