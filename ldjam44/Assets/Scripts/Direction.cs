using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardinalDirection
{
	EAST,
	NORTH,
	WEST,
	SOUTH
}

public static class DirectionUtils
{
	static Vector3[] directionVec = {
			new Vector3( 1, 0 ),
			new Vector3( 0, 1 ),
			new Vector3( -1, 0 ),
			new Vector3( 0, -1 )
		};

	public static Vector3 CardinalDirectionToVec(CardinalDirection dir)
	{
		return directionVec[(int)dir];
	}

	public static CardinalDirection CoordinatesToCardinalDirection(Vector2 dir)
	{
		return (CardinalDirection)CoordinatesToSector(dir, 4);
	}

	private static int CoordinatesToSector(Vector2 dir, int numSectors)
	{
		Vector2 norm = dir.normalized;
		float angle = (float)Math.Atan2(norm.y, norm.x);
		return (int)Math.Round(((numSectors * angle) / (2 * Math.PI)) + numSectors) % numSectors;
	}
}