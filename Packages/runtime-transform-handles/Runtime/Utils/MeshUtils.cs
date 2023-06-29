using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
	/**
     * Created by Peter @sHTiF Stefcek 20.10.2020, some functions based on Unity wiki
     */
	internal static class MeshUtils
	{
		public static Mesh CreateArc(Vector3 pCenter, Vector3 pStartPoint, Vector3 pAxis, float pRadius, float pAngle, int pSegmentCount)
		{
			var mesh = new Mesh();
			
			var vertices = new Vector3[pSegmentCount+2];

			var startVector = (pStartPoint - pCenter).normalized * pRadius;
			for (var i = 0; i<=pSegmentCount; i++)
			{
				var rad = (float) i / pSegmentCount * pAngle;
				var v = Quaternion.AngleAxis(rad*180f/Mathf.PI, pAxis) * startVector;
				vertices[i] = v + pCenter;
			}
			vertices[pSegmentCount+1] = pCenter;
			
			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < normals.Length; n++ )
				normals[n] = Vector3.up;

			var uvs = new Vector2[vertices.Length];
			for (var i = 0; i<=pSegmentCount; i++)
			{
				var rad = (float) i / pSegmentCount * pAngle;
				uvs[i] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			}
			uvs[pSegmentCount + 1] = Vector2.one / 2f;
			
			var triangles = new int[ pSegmentCount * 3 ];
			for (var i = 0; i < pSegmentCount; i++)
			{
				var index = i * 3;
				triangles[index] = pSegmentCount+1;
				triangles[index+1] = i;
				triangles[index+2] = i + 1;
			}
			
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
 
			mesh.RecalculateBounds();
			mesh.Optimize();
			
			return mesh;
		}
		
		public static Mesh CreateArc(float pRadius, float pAngle, int pSegmentCount)
		{
			var mesh = new Mesh();
			
			var vertices = new Vector3[pSegmentCount+2];
			
			for (var i = 0; i<=pSegmentCount; i++)
			{
				var rad = (float) i / pSegmentCount * pAngle;
				vertices[i] = new Vector3(Mathf.Cos(rad) * pRadius, 0f, Mathf.Sin(rad) * pRadius);
			}
			vertices[pSegmentCount+1] = Vector3.zero;
			
			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < normals.Length; n++ )
				normals[n] = Vector3.up;

			var uvs = new Vector2[vertices.Length];
			for (var i = 0; i<=pSegmentCount; i++)
			{
				var rad = (float) i / pSegmentCount * pAngle;
				uvs[i] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			}
			uvs[pSegmentCount + 1] = Vector2.one / 2f;
			
			var triangles = new int[ pSegmentCount * 3 ];
			for (var i = 0; i < pSegmentCount; i++)
			{
				var index = i * 3;
				triangles[index] = pSegmentCount+1;
				triangles[index+1] = i;
				triangles[index+2] = i + 1;
			}
			
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
 
			mesh.RecalculateBounds();
			mesh.Optimize();
			
			return mesh;
		}
		
		public static Mesh CreateGrid(float pWidth, float pHeight, int pSegmentsX = 1, int pSegmentsY = 1)
		{
			var mesh = new Mesh();

			var resX = pSegmentsX + 1;
			var resZ = pSegmentsY + 1;
			
			var vertices = new Vector3[ resX * resZ ];
			for(var z = 0; z < resZ; z++)
			{
				var zPos = ((float)z / (resZ - 1) - .5f) * pHeight;
				for(var x = 0; x < resX; x++)
				{
					var xPos = ((float)x / (resX - 1) - .5f) * pWidth;
					vertices[ x + z * resX ] = new Vector3( xPos, 0f, zPos );
				}
			}

			
			var normals = new Vector3[ vertices.Length ];
			for( var n = 0; n < normals.Length; n++ )
				normals[n] = Vector3.up;

			
			var uvs = new Vector2[ vertices.Length ];
			for(var v = 0; v < resZ; v++)
			{
				for(var u = 0; u < resX; u++)
				{
					uvs[ u + v * resX ] = new Vector2( (float)u / (resX - 1), (float)v / (resZ - 1) );
				}
			}


			var faceCount = (resX - 1) * (resZ - 1);
			var triangles = new int[ faceCount * 6 ];
			var t = 0;
			for(var face = 0; face < faceCount; face++ )
			{
				// Retrieve lower left corner from face ind
				var i = face % (resX - 1) + (face / (resZ - 1) * resX);
 
				triangles[t++] = i + resX;
				triangles[t++] = i + 1;
				triangles[t++] = i;
 
				triangles[t++] = i + resX;	
				triangles[t++] = i + resX + 1;
				triangles[t++] = i + 1; 
			}

 
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
 
			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}
		
		public static Mesh CreateBox(float pWidth, float pHeight, float pDepth)
		{
			var mesh = new Mesh();

			var v0 = new Vector3(-pDepth * .5f, -pWidth * .5f, pHeight * .5f);
			var v1 = new Vector3(pDepth * .5f, -pWidth * .5f, pHeight * .5f);
			var v2 = new Vector3(pDepth * .5f, -pWidth * .5f, -pHeight * .5f);
			var v3 = new Vector3(-pDepth * .5f, -pWidth * .5f, -pHeight * .5f);

			var v4 = new Vector3(-pDepth * .5f, pWidth * .5f, pHeight * .5f);
			var v5 = new Vector3(pDepth * .5f, pWidth * .5f, pHeight * .5f);
			var v6 = new Vector3(pDepth * .5f, pWidth * .5f, -pHeight * .5f);
			var v7 = new Vector3(-pDepth * .5f, pWidth * .5f, -pHeight * .5f);

			var vertices = new Vector3[]
			{
				// Bottom
				v0, v1, v2, v3,

				// Left
				v7, v4, v0, v3,

				// Front
				v4, v5, v1, v0,

				// Back
				v6, v7, v3, v2,

				// Right
				v5, v6, v2, v1,

				// Top
				v7, v6, v5, v4
			};

			var up = Vector3.up;
			var down = Vector3.down;
			var front = Vector3.forward;
			var back = Vector3.back;
			var left = Vector3.left;
			var right = Vector3.right;

			var normals = new Vector3[]
			{
				down, down, down, down,

				left, left, left, left,

				front, front, front, front,

				back, back, back, back,

				right, right, right, right,

				up, up, up, up
			};

			var _00 = new Vector2(0f, 0f);
			var _10 = new Vector2(1f, 0f);
			var _01 = new Vector2(0f, 1f);
			var _11 = new Vector2(1f, 1f);

			var uvs = new Vector2[]
			{
				// Bottom
				_11, _01, _00, _10,

				// Left
				_11, _01, _00, _10,

				// Front
				_11, _01, _00, _10,

				// Back
				_11, _01, _00, _10,

				// Right
				_11, _01, _00, _10,

				// Top
				_11, _01, _00, _10,
			};

			var triangles = new int[]
			{
				// Bottom
				3, 1, 0,
				3, 2, 1,

				// Left
				3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
				3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,

				// Front
				3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
				3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,

				// Back
				3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
				3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,

				// Right
				3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
				3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,

				// Top
				3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
				3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
			};

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}

		public static Mesh CreateCone(float pHeight, float pBottomRadius, float pTopRadius, int pSideCount,
			int pHeightSegmentCount)
		{
			var mesh = new Mesh();

			var vertexCapCount = pSideCount + 1;

			// bottom + top + sides
			var vertices =
				new Vector3[vertexCapCount + vertexCapCount + pSideCount * pHeightSegmentCount * 2 + 2];
			var vert = 0;
			var _2pi = Mathf.PI * 2f;

			// Bottom cap
			vertices[vert++] = new Vector3(0f, 0f, 0f);
			while (vert <= pSideCount)
			{
				var rad = (float) vert / pSideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * pBottomRadius, 0f, Mathf.Sin(rad) * pBottomRadius);
				vert++;
			}

			// Top cap
			vertices[vert++] = new Vector3(0f, pHeight, 0f);
			while (vert <= pSideCount * 2 + 1)
			{
				var rad = (float) (vert - pSideCount - 1) / pSideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * pTopRadius, pHeight, Mathf.Sin(rad) * pTopRadius);
				vert++;
			}

			// Sides
			var v = 0;
			while (vert <= vertices.Length - 4)
			{
				var rad = (float) v / pSideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * pTopRadius, pHeight, Mathf.Sin(rad) * pTopRadius);
				vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * pBottomRadius, 0, Mathf.Sin(rad) * pBottomRadius);
				vert += 2;
				v++;
			}

			vertices[vert] = vertices[pSideCount * 2 + 2];
			vertices[vert + 1] = vertices[pSideCount * 2 + 3];


			// bottom + top + sides
			var normals = new Vector3[vertices.Length];
			vert = 0;

			// Bottom cap
			while (vert <= pSideCount)
			{
				normals[vert++] = Vector3.down;
			}

			// Top cap
			while (vert <= pSideCount * 2 + 1)
			{
				normals[vert++] = Vector3.up;
			}

			// Sides
			v = 0;
			while (vert <= vertices.Length - 4)
			{
				var rad = (float) v / pSideCount * _2pi;
				var cos = Mathf.Cos(rad);
				var sin = Mathf.Sin(rad);

				normals[vert] = new Vector3(cos, 0f, sin);
				normals[vert + 1] = normals[vert];

				vert += 2;
				v++;
			}

			normals[vert] = normals[pSideCount * 2 + 2];
			normals[vert + 1] = normals[pSideCount * 2 + 3];


			var uvs = new Vector2[vertices.Length];

			// Bottom cap
			var u = 0;
			uvs[u++] = new Vector2(0.5f, 0.5f);
			while (u <= pSideCount)
			{
				var rad = (float) u / pSideCount * _2pi;
				uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
				u++;
			}

			// Top cap
			uvs[u++] = new Vector2(0.5f, 0.5f);
			while (u <= pSideCount * 2 + 1)
			{
				var rad = (float) u / pSideCount * _2pi;
				uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
				u++;
			}

			// Sides
			var uSides = 0;
			while (u <= uvs.Length - 4)
			{
				var t = (float) uSides / pSideCount;
				uvs[u] = new Vector3(t, 1f);
				uvs[u + 1] = new Vector3(t, 0f);
				u += 2;
				uSides++;
			}

			uvs[u] = new Vector2(1f, 1f);
			uvs[u + 1] = new Vector2(1f, 0f);

			var triangleCount = pSideCount + pSideCount + pSideCount * 2;
			var triangles = new int[triangleCount * 3 + 3];

			// Bottom cap
			var tri = 0;
			var i = 0;
			while (tri < pSideCount - 1)
			{
				triangles[i] = 0;
				triangles[i + 1] = tri + 1;
				triangles[i + 2] = tri + 2;
				tri++;
				i += 3;
			}

			triangles[i] = 0;
			triangles[i + 1] = tri + 1;
			triangles[i + 2] = 1;
			tri++;
			i += 3;

			// Top cap
			while (tri < pSideCount * 2)
			{
				triangles[i] = tri + 2;
				triangles[i + 1] = tri + 1;
				triangles[i + 2] = vertexCapCount;
				tri++;
				i += 3;
			}

			triangles[i] = vertexCapCount + 1;
			triangles[i + 1] = tri + 1;
			triangles[i + 2] = vertexCapCount;
			tri++;
			i += 3;
			tri++;

			// Sides
			while (tri <= triangleCount)
			{
				triangles[i] = tri + 2;
				triangles[i + 1] = tri + 1;
				triangles[i + 2] = tri + 0;
				tri++;
				i += 3;

				triangles[i] = tri + 1;
				triangles[i + 1] = tri + 2;
				triangles[i + 2] = tri + 0;
				tri++;
				i += 3;
			}


			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}

		static public Mesh CreateTube(float pHeight, int pSideCount, float pBottomRadius, float pBottomThickness,
			float pTopRadius, float pTopThickness)
		{
			var mesh = new Mesh();

			var vertexCapCount = pSideCount * 2 + 2;
			var vertexSideCount = pSideCount * 2 + 2;


			// bottom + top + sides
			var vertices = new Vector3[vertexCapCount * 2 + vertexSideCount * 2];
			var vert = 0;
			var _2pi = Mathf.PI * 2f;

			// Bottom cap
			var sideCounter = 0;
			while (vert < vertexCapCount)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);
				vertices[vert] = new Vector3(cos * (pBottomRadius - pBottomThickness * .5f), 0f,
					sin * (pBottomRadius - pBottomThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (pBottomRadius + pBottomThickness * .5f), 0f,
					sin * (pBottomRadius + pBottomThickness * .5f));
				vert += 2;
			}

			// Top cap
			sideCounter = 0;
			while (vert < vertexCapCount * 2)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);
				vertices[vert] = new Vector3(cos * (pTopRadius - pTopThickness * .5f), pHeight,
					sin * (pTopRadius - pTopThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (pTopRadius + pTopThickness * .5f), pHeight,
					sin * (pTopRadius + pTopThickness * .5f));
				vert += 2;
			}

			// Sides (out)
			sideCounter = 0;
			while (vert < vertexCapCount * 2 + vertexSideCount)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);

				vertices[vert] = new Vector3(cos * (pTopRadius + pTopThickness * .5f), pHeight,
					sin * (pTopRadius + pTopThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (pBottomRadius + pBottomThickness * .5f), 0,
					sin * (pBottomRadius + pBottomThickness * .5f));
				vert += 2;
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);

				vertices[vert] = new Vector3(cos * (pTopRadius - pTopThickness * .5f), pHeight,
					sin * (pTopRadius - pTopThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (pBottomRadius - pBottomThickness * .5f), 0,
					sin * (pBottomRadius - pBottomThickness * .5f));
				vert += 2;
			}


			// bottom + top + sides
			var normals = new Vector3[vertices.Length];
			vert = 0;

			// Bottom cap
			while (vert < vertexCapCount)
			{
				normals[vert++] = Vector3.down;
			}

			// Top cap
			while (vert < vertexCapCount * 2)
			{
				normals[vert++] = Vector3.up;
			}

			// Sides (out)
			sideCounter = 0;
			while (vert < vertexCapCount * 2 + vertexSideCount)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;

				normals[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
				normals[vert + 1] = normals[vert];
				vert += 2;
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				sideCounter = sideCounter == pSideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / pSideCount * _2pi;

				normals[vert] = -(new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1)));
				normals[vert + 1] = normals[vert];
				vert += 2;
			}

			var uvs = new Vector2[vertices.Length];

			vert = 0;
			// Bottom cap
			sideCounter = 0;
			while (vert < vertexCapCount)
			{
				var t = (float) (sideCounter++) / pSideCount;
				uvs[vert++] = new Vector2(0f, t);
				uvs[vert++] = new Vector2(1f, t);
			}

			// Top cap
			sideCounter = 0;
			while (vert < vertexCapCount * 2)
			{
				var t = (float) (sideCounter++) / pSideCount;
				uvs[vert++] = new Vector2(0f, t);
				uvs[vert++] = new Vector2(1f, t);
			}

			// Sides (out)
			sideCounter = 0;
			while (vert < vertexCapCount * 2 + vertexSideCount)
			{
				var t = (float) (sideCounter++) / pSideCount;
				uvs[vert++] = new Vector2(t, 0f);
				uvs[vert++] = new Vector2(t, 1f);
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				var t = (float) (sideCounter++) / pSideCount;
				uvs[vert++] = new Vector2(t, 0f);
				uvs[vert++] = new Vector2(t, 1f);
			}

			var faceCount = pSideCount * 4;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[indexCount];

			// Bottom cap
			var i = 0;
			sideCounter = 0;
			while (sideCounter < pSideCount)
			{
				var current = sideCounter * 2;
				var next = sideCounter * 2 + 2;

				triangles[i++] = next + 1;
				triangles[i++] = next;
				triangles[i++] = current;

				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
				triangles[i++] = current;

				sideCounter++;
			}

			// Top cap
			while (sideCounter < pSideCount * 2)
			{
				var current = sideCounter * 2 + 2;
				var next = sideCounter * 2 + 4;

				triangles[i++] = current;
				triangles[i++] = next;
				triangles[i++] = next + 1;

				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = current + 1;

				sideCounter++;
			}

			// Sides (out)
			while (sideCounter < pSideCount * 3)
			{
				var current = sideCounter * 2 + 4;
				var next = sideCounter * 2 + 6;

				triangles[i++] = current;
				triangles[i++] = next;
				triangles[i++] = next + 1;

				triangles[i++] = current;
				triangles[i++] = next + 1;
				triangles[i++] = current + 1;

				sideCounter++;
			}


			// Sides (in)
			while (sideCounter < pSideCount * 4)
			{
				var current = sideCounter * 2 + 6;
				var next = sideCounter * 2 + 8;

				triangles[i++] = next + 1;
				triangles[i++] = next;
				triangles[i++] = current;

				triangles[i++] = current + 1;
				triangles[i++] = next + 1;
				triangles[i++] = current;

				sideCounter++;
			}

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}

		static public Mesh CreateTorus(float pRadius, float pThickness, int pRadiusSegmentCount, int pSideCount)
		{
			var mesh = new Mesh();


			var vertices = new Vector3[(pRadiusSegmentCount + 1) * (pSideCount + 1)];
			var _2pi = Mathf.PI * 2f;
			for (var seg = 0; seg <= pRadiusSegmentCount; seg++)
			{
				var currSeg = seg == pRadiusSegmentCount ? 0 : seg;

				var t1 = (float) currSeg / pRadiusSegmentCount * _2pi;
				var r1 = new Vector3(Mathf.Cos(t1) * pRadius, 0f, Mathf.Sin(t1) * pRadius);

				for (var side = 0; side <= pSideCount; side++)
				{
					var currSide = side == pSideCount ? 0 : side;

					var normale = Vector3.Cross(r1, Vector3.up);
					var t2 = (float) currSide / pSideCount * _2pi;
					var r2 = Quaternion.AngleAxis(-t1 * Mathf.Rad2Deg, Vector3.up) *
					         new Vector3(Mathf.Sin(t2) * pThickness, Mathf.Cos(t2) * pThickness);

					vertices[side + seg * (pSideCount + 1)] = r1 + r2;
				}
			}


			var normals = new Vector3[vertices.Length];
			for (var seg = 0; seg <= pRadiusSegmentCount; seg++)
			{
				var currSeg = seg == pRadiusSegmentCount ? 0 : seg;

				var t1 = (float) currSeg / pRadiusSegmentCount * _2pi;
				var r1 = new Vector3(Mathf.Cos(t1) * pRadius, 0f, Mathf.Sin(t1) * pRadius);

				for (var side = 0; side <= pSideCount; side++)
				{
					normals[side + seg * (pSideCount + 1)] =
						(vertices[side + seg * (pSideCount + 1)] - r1).normalized;
				}
			}


			var uvs = new Vector2[vertices.Length];
			for (var seg = 0; seg <= pRadiusSegmentCount; seg++)
			for (var side = 0; side <= pSideCount; side++)
				uvs[side + seg * (pSideCount + 1)] =
					new Vector2((float) seg / pRadiusSegmentCount, (float) side / pSideCount);


			var faceCount = vertices.Length;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[indexCount];

			var i = 0;
			for (var seg = 0; seg <= pRadiusSegmentCount; seg++)
			{
				for (var side = 0; side <= pSideCount - 1; side++)
				{
					var current = side + seg * (pSideCount + 1);
					var next = side + (seg < (pRadiusSegmentCount) ? (seg + 1) * (pSideCount + 1) : 0);

					if (i < triangles.Length - 6)
					{
						triangles[i++] = current;
						triangles[i++] = next;
						triangles[i++] = next + 1;

						triangles[i++] = current;
						triangles[i++] = next + 1;
						triangles[i++] = current + 1;
					}
				}
			}

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}

		static public Mesh CreateSphere(float pRadius, int pLongitudeCount, int pLattitudeCount)
		{
			var mesh = new Mesh();
			mesh.Clear();
			
			var vertices = new Vector3[(pLongitudeCount+1) * pLattitudeCount + 2];
			var pi = Mathf.PI;
			var _2pi = pi * 2f;
			 
			vertices[0] = Vector3.up * pRadius;
			for( var lat = 0; lat < pLattitudeCount; lat++ )
			{
				var a1 = pi * (float)(lat+1) / (pLattitudeCount+1);
				var sin1 = Mathf.Sin(a1);
				var cos1 = Mathf.Cos(a1);
			 
				for( var lon = 0; lon <= pLongitudeCount; lon++ )
				{
					var a2 = _2pi * (float)(lon == pLongitudeCount ? 0 : lon) / pLongitudeCount;
					var sin2 = Mathf.Sin(a2);
					var cos2 = Mathf.Cos(a2);
			 
					vertices[ lon + lat * (pLongitudeCount + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * pRadius;
				}
			}
			vertices[vertices.Length-1] = Vector3.up * -pRadius;


			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < vertices.Length; n++ )
				normals[n] = vertices[n].normalized;

			
			var uvs = new Vector2[vertices.Length];
			uvs[0] = Vector2.up;
			uvs[uvs.Length-1] = Vector2.zero;
			for( var lat = 0; lat < pLattitudeCount; lat++ )
				for( var lon = 0; lon <= pLongitudeCount; lon++ )
					uvs[lon + lat * (pLongitudeCount + 1) + 1] = new Vector2( (float)lon / pLongitudeCount, 1f - (float)(lat+1) / (pLattitudeCount+1) );
			
			var faceCount = vertices.Length;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[ indexCount ];
			 
			//Top Cap
			var i = 0;
			for( var lon = 0; lon < pLongitudeCount; lon++ )
			{
				triangles[i++] = lon+2;
				triangles[i++] = lon+1;
				triangles[i++] = 0;
			}
			 
			//Middle
			for( var lat = 0; lat < pLattitudeCount - 1; lat++ )
			{
				for( var lon = 0; lon < pLongitudeCount; lon++ )
				{
					var current = lon + lat * (pLongitudeCount + 1) + 1;
					var next = current + pLongitudeCount + 1;
			 
					triangles[i++] = current;
					triangles[i++] = current + 1;
					triangles[i++] = next + 1;
			 
					triangles[i++] = current;
					triangles[i++] = next + 1;
					triangles[i++] = next;
				}
			}
			 
			//Bottom Cap
			for( var lon = 0; lon < pLongitudeCount; lon++ )
			{
				triangles[i++] = vertices.Length - 1;
				triangles[i++] = vertices.Length - (lon+2) - 1;
				triangles[i++] = vertices.Length - (lon+1) - 1;
			}

			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.triangles = triangles;
			 
			mesh.RecalculateBounds();
			mesh.Optimize();

			return mesh;
		}
	}
}