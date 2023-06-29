using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
	/**
     * Created by Peter @sHTiF Stefcek 20.10.2020, some functions based on Unity wiki
     */
	public class MeshUtils
	{
		static public Mesh CreateArc(Vector3 p_center, Vector3 p_startPoint, Vector3 p_axis, float p_radius, float p_angle, int p_segmentCount)
		{
			var mesh = new Mesh();
			
			var vertices = new Vector3[p_segmentCount+2];

			var startVector = (p_startPoint - p_center).normalized * p_radius;
			for (var i = 0; i<=p_segmentCount; i++)
			{
				var rad = (float) i / p_segmentCount * p_angle;
				var v = Quaternion.AngleAxis(rad*180f/Mathf.PI, p_axis) * startVector;
				vertices[i] = v + p_center;
			}
			vertices[p_segmentCount+1] = p_center;
			
			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < normals.Length; n++ )
				normals[n] = Vector3.up;

			var uvs = new Vector2[vertices.Length];
			for (var i = 0; i<=p_segmentCount; i++)
			{
				var rad = (float) i / p_segmentCount * p_angle;
				uvs[i] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			}
			uvs[p_segmentCount + 1] = Vector2.one / 2f;
			
			var triangles = new int[ p_segmentCount * 3 ];
			for (var i = 0; i < p_segmentCount; i++)
			{
				var index = i * 3;
				triangles[index] = p_segmentCount+1;
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
		
		static public Mesh CreateArc(float p_radius, float p_angle, int p_segmentCount)
		{
			var mesh = new Mesh();
			
			var vertices = new Vector3[p_segmentCount+2];
			
			for (var i = 0; i<=p_segmentCount; i++)
			{
				var rad = (float) i / p_segmentCount * p_angle;
				vertices[i] = new Vector3(Mathf.Cos(rad) * p_radius, 0f, Mathf.Sin(rad) * p_radius);
			}
			vertices[p_segmentCount+1] = Vector3.zero;
			
			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < normals.Length; n++ )
				normals[n] = Vector3.up;

			var uvs = new Vector2[vertices.Length];
			for (var i = 0; i<=p_segmentCount; i++)
			{
				var rad = (float) i / p_segmentCount * p_angle;
				uvs[i] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
			}
			uvs[p_segmentCount + 1] = Vector2.one / 2f;
			
			var triangles = new int[ p_segmentCount * 3 ];
			for (var i = 0; i < p_segmentCount; i++)
			{
				var index = i * 3;
				triangles[index] = p_segmentCount+1;
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
		
		static public Mesh CreateGrid(float p_width, float p_height, int p_segmentsX = 1, int p_segmentsY = 1)
		{
			var mesh = new Mesh();

			var resX = p_segmentsX + 1;
			var resZ = p_segmentsY + 1;
			
			var vertices = new Vector3[ resX * resZ ];
			for(var z = 0; z < resZ; z++)
			{
				var zPos = ((float)z / (resZ - 1) - .5f) * p_height;
				for(var x = 0; x < resX; x++)
				{
					var xPos = ((float)x / (resX - 1) - .5f) * p_width;
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
		
		static public Mesh CreateBox(float p_width, float p_height, float p_depth)
		{
			var mesh = new Mesh();

			var v0 = new Vector3(-p_depth * .5f, -p_width * .5f, p_height * .5f);
			var v1 = new Vector3(p_depth * .5f, -p_width * .5f, p_height * .5f);
			var v2 = new Vector3(p_depth * .5f, -p_width * .5f, -p_height * .5f);
			var v3 = new Vector3(-p_depth * .5f, -p_width * .5f, -p_height * .5f);

			var v4 = new Vector3(-p_depth * .5f, p_width * .5f, p_height * .5f);
			var v5 = new Vector3(p_depth * .5f, p_width * .5f, p_height * .5f);
			var v6 = new Vector3(p_depth * .5f, p_width * .5f, -p_height * .5f);
			var v7 = new Vector3(-p_depth * .5f, p_width * .5f, -p_height * .5f);

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

		static public Mesh CreateCone(float p_height, float p_bottomRadius, float p_topRadius, int p_sideCount,
			int p_heightSegmentCount)
		{
			var mesh = new Mesh();

			var vertexCapCount = p_sideCount + 1;

			// bottom + top + sides
			var vertices =
				new Vector3[vertexCapCount + vertexCapCount + p_sideCount * p_heightSegmentCount * 2 + 2];
			var vert = 0;
			var _2pi = Mathf.PI * 2f;

			// Bottom cap
			vertices[vert++] = new Vector3(0f, 0f, 0f);
			while (vert <= p_sideCount)
			{
				var rad = (float) vert / p_sideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * p_bottomRadius, 0f, Mathf.Sin(rad) * p_bottomRadius);
				vert++;
			}

			// Top cap
			vertices[vert++] = new Vector3(0f, p_height, 0f);
			while (vert <= p_sideCount * 2 + 1)
			{
				var rad = (float) (vert - p_sideCount - 1) / p_sideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * p_topRadius, p_height, Mathf.Sin(rad) * p_topRadius);
				vert++;
			}

			// Sides
			var v = 0;
			while (vert <= vertices.Length - 4)
			{
				var rad = (float) v / p_sideCount * _2pi;
				vertices[vert] = new Vector3(Mathf.Cos(rad) * p_topRadius, p_height, Mathf.Sin(rad) * p_topRadius);
				vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * p_bottomRadius, 0, Mathf.Sin(rad) * p_bottomRadius);
				vert += 2;
				v++;
			}

			vertices[vert] = vertices[p_sideCount * 2 + 2];
			vertices[vert + 1] = vertices[p_sideCount * 2 + 3];


			// bottom + top + sides
			var normals = new Vector3[vertices.Length];
			vert = 0;

			// Bottom cap
			while (vert <= p_sideCount)
			{
				normals[vert++] = Vector3.down;
			}

			// Top cap
			while (vert <= p_sideCount * 2 + 1)
			{
				normals[vert++] = Vector3.up;
			}

			// Sides
			v = 0;
			while (vert <= vertices.Length - 4)
			{
				var rad = (float) v / p_sideCount * _2pi;
				var cos = Mathf.Cos(rad);
				var sin = Mathf.Sin(rad);

				normals[vert] = new Vector3(cos, 0f, sin);
				normals[vert + 1] = normals[vert];

				vert += 2;
				v++;
			}

			normals[vert] = normals[p_sideCount * 2 + 2];
			normals[vert + 1] = normals[p_sideCount * 2 + 3];


			var uvs = new Vector2[vertices.Length];

			// Bottom cap
			var u = 0;
			uvs[u++] = new Vector2(0.5f, 0.5f);
			while (u <= p_sideCount)
			{
				var rad = (float) u / p_sideCount * _2pi;
				uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
				u++;
			}

			// Top cap
			uvs[u++] = new Vector2(0.5f, 0.5f);
			while (u <= p_sideCount * 2 + 1)
			{
				var rad = (float) u / p_sideCount * _2pi;
				uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
				u++;
			}

			// Sides
			var u_sides = 0;
			while (u <= uvs.Length - 4)
			{
				var t = (float) u_sides / p_sideCount;
				uvs[u] = new Vector3(t, 1f);
				uvs[u + 1] = new Vector3(t, 0f);
				u += 2;
				u_sides++;
			}

			uvs[u] = new Vector2(1f, 1f);
			uvs[u + 1] = new Vector2(1f, 0f);

			var triangleCount = p_sideCount + p_sideCount + p_sideCount * 2;
			var triangles = new int[triangleCount * 3 + 3];

			// Bottom cap
			var tri = 0;
			var i = 0;
			while (tri < p_sideCount - 1)
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
			while (tri < p_sideCount * 2)
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

		static public Mesh CreateTube(float p_height, int p_sideCount, float p_bottomRadius, float p_bottomThickness,
			float p_topRadius, float p_topThickness)
		{
			var mesh = new Mesh();

			var vertexCapCount = p_sideCount * 2 + 2;
			var vertexSideCount = p_sideCount * 2 + 2;


			// bottom + top + sides
			var vertices = new Vector3[vertexCapCount * 2 + vertexSideCount * 2];
			var vert = 0;
			var _2pi = Mathf.PI * 2f;

			// Bottom cap
			var sideCounter = 0;
			while (vert < vertexCapCount)
			{
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);
				vertices[vert] = new Vector3(cos * (p_bottomRadius - p_bottomThickness * .5f), 0f,
					sin * (p_bottomRadius - p_bottomThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (p_bottomRadius + p_bottomThickness * .5f), 0f,
					sin * (p_bottomRadius + p_bottomThickness * .5f));
				vert += 2;
			}

			// Top cap
			sideCounter = 0;
			while (vert < vertexCapCount * 2)
			{
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);
				vertices[vert] = new Vector3(cos * (p_topRadius - p_topThickness * .5f), p_height,
					sin * (p_topRadius - p_topThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (p_topRadius + p_topThickness * .5f), p_height,
					sin * (p_topRadius + p_topThickness * .5f));
				vert += 2;
			}

			// Sides (out)
			sideCounter = 0;
			while (vert < vertexCapCount * 2 + vertexSideCount)
			{
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);

				vertices[vert] = new Vector3(cos * (p_topRadius + p_topThickness * .5f), p_height,
					sin * (p_topRadius + p_topThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (p_bottomRadius + p_bottomThickness * .5f), 0,
					sin * (p_bottomRadius + p_bottomThickness * .5f));
				vert += 2;
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;
				var cos = Mathf.Cos(r1);
				var sin = Mathf.Sin(r1);

				vertices[vert] = new Vector3(cos * (p_topRadius - p_topThickness * .5f), p_height,
					sin * (p_topRadius - p_topThickness * .5f));
				vertices[vert + 1] = new Vector3(cos * (p_bottomRadius - p_bottomThickness * .5f), 0,
					sin * (p_bottomRadius - p_bottomThickness * .5f));
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
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;

				normals[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
				normals[vert + 1] = normals[vert];
				vert += 2;
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				sideCounter = sideCounter == p_sideCount ? 0 : sideCounter;

				var r1 = (float) (sideCounter++) / p_sideCount * _2pi;

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
				var t = (float) (sideCounter++) / p_sideCount;
				uvs[vert++] = new Vector2(0f, t);
				uvs[vert++] = new Vector2(1f, t);
			}

			// Top cap
			sideCounter = 0;
			while (vert < vertexCapCount * 2)
			{
				var t = (float) (sideCounter++) / p_sideCount;
				uvs[vert++] = new Vector2(0f, t);
				uvs[vert++] = new Vector2(1f, t);
			}

			// Sides (out)
			sideCounter = 0;
			while (vert < vertexCapCount * 2 + vertexSideCount)
			{
				var t = (float) (sideCounter++) / p_sideCount;
				uvs[vert++] = new Vector2(t, 0f);
				uvs[vert++] = new Vector2(t, 1f);
			}

			// Sides (in)
			sideCounter = 0;
			while (vert < vertices.Length)
			{
				var t = (float) (sideCounter++) / p_sideCount;
				uvs[vert++] = new Vector2(t, 0f);
				uvs[vert++] = new Vector2(t, 1f);
			}

			var faceCount = p_sideCount * 4;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[indexCount];

			// Bottom cap
			var i = 0;
			sideCounter = 0;
			while (sideCounter < p_sideCount)
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
			while (sideCounter < p_sideCount * 2)
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
			while (sideCounter < p_sideCount * 3)
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
			while (sideCounter < p_sideCount * 4)
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

		static public Mesh CreateTorus(float p_radius, float p_thickness, int p_radiusSegmentCount, int p_sideCount)
		{
			var mesh = new Mesh();


			var vertices = new Vector3[(p_radiusSegmentCount + 1) * (p_sideCount + 1)];
			var _2pi = Mathf.PI * 2f;
			for (var seg = 0; seg <= p_radiusSegmentCount; seg++)
			{
				var currSeg = seg == p_radiusSegmentCount ? 0 : seg;

				var t1 = (float) currSeg / p_radiusSegmentCount * _2pi;
				var r1 = new Vector3(Mathf.Cos(t1) * p_radius, 0f, Mathf.Sin(t1) * p_radius);

				for (var side = 0; side <= p_sideCount; side++)
				{
					var currSide = side == p_sideCount ? 0 : side;

					var normale = Vector3.Cross(r1, Vector3.up);
					var t2 = (float) currSide / p_sideCount * _2pi;
					var r2 = Quaternion.AngleAxis(-t1 * Mathf.Rad2Deg, Vector3.up) *
					         new Vector3(Mathf.Sin(t2) * p_thickness, Mathf.Cos(t2) * p_thickness);

					vertices[side + seg * (p_sideCount + 1)] = r1 + r2;
				}
			}


			var normals = new Vector3[vertices.Length];
			for (var seg = 0; seg <= p_radiusSegmentCount; seg++)
			{
				var currSeg = seg == p_radiusSegmentCount ? 0 : seg;

				var t1 = (float) currSeg / p_radiusSegmentCount * _2pi;
				var r1 = new Vector3(Mathf.Cos(t1) * p_radius, 0f, Mathf.Sin(t1) * p_radius);

				for (var side = 0; side <= p_sideCount; side++)
				{
					normals[side + seg * (p_sideCount + 1)] =
						(vertices[side + seg * (p_sideCount + 1)] - r1).normalized;
				}
			}


			var uvs = new Vector2[vertices.Length];
			for (var seg = 0; seg <= p_radiusSegmentCount; seg++)
			for (var side = 0; side <= p_sideCount; side++)
				uvs[side + seg * (p_sideCount + 1)] =
					new Vector2((float) seg / p_radiusSegmentCount, (float) side / p_sideCount);


			var faceCount = vertices.Length;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[indexCount];

			var i = 0;
			for (var seg = 0; seg <= p_radiusSegmentCount; seg++)
			{
				for (var side = 0; side <= p_sideCount - 1; side++)
				{
					var current = side + seg * (p_sideCount + 1);
					var next = side + (seg < (p_radiusSegmentCount) ? (seg + 1) * (p_sideCount + 1) : 0);

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

		static public Mesh CreateSphere(float p_radius, int p_longitudeCount, int p_lattitudeCount)
		{
			var mesh = new Mesh();
			mesh.Clear();
			
			var vertices = new Vector3[(p_longitudeCount+1) * p_lattitudeCount + 2];
			var _pi = Mathf.PI;
			var _2pi = _pi * 2f;
			 
			vertices[0] = Vector3.up * p_radius;
			for( var lat = 0; lat < p_lattitudeCount; lat++ )
			{
				var a1 = _pi * (float)(lat+1) / (p_lattitudeCount+1);
				var sin1 = Mathf.Sin(a1);
				var cos1 = Mathf.Cos(a1);
			 
				for( var lon = 0; lon <= p_longitudeCount; lon++ )
				{
					var a2 = _2pi * (float)(lon == p_longitudeCount ? 0 : lon) / p_longitudeCount;
					var sin2 = Mathf.Sin(a2);
					var cos2 = Mathf.Cos(a2);
			 
					vertices[ lon + lat * (p_longitudeCount + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * p_radius;
				}
			}
			vertices[vertices.Length-1] = Vector3.up * -p_radius;


			var normals = new Vector3[vertices.Length];
			for( var n = 0; n < vertices.Length; n++ )
				normals[n] = vertices[n].normalized;

			
			var uvs = new Vector2[vertices.Length];
			uvs[0] = Vector2.up;
			uvs[uvs.Length-1] = Vector2.zero;
			for( var lat = 0; lat < p_lattitudeCount; lat++ )
				for( var lon = 0; lon <= p_longitudeCount; lon++ )
					uvs[lon + lat * (p_longitudeCount + 1) + 1] = new Vector2( (float)lon / p_longitudeCount, 1f - (float)(lat+1) / (p_lattitudeCount+1) );
			
			var faceCount = vertices.Length;
			var triangleCount = faceCount * 2;
			var indexCount = triangleCount * 3;
			var triangles = new int[ indexCount ];
			 
			//Top Cap
			var i = 0;
			for( var lon = 0; lon < p_longitudeCount; lon++ )
			{
				triangles[i++] = lon+2;
				triangles[i++] = lon+1;
				triangles[i++] = 0;
			}
			 
			//Middle
			for( var lat = 0; lat < p_lattitudeCount - 1; lat++ )
			{
				for( var lon = 0; lon < p_longitudeCount; lon++ )
				{
					var current = lon + lat * (p_longitudeCount + 1) + 1;
					var next = current + p_longitudeCount + 1;
			 
					triangles[i++] = current;
					triangles[i++] = current + 1;
					triangles[i++] = next + 1;
			 
					triangles[i++] = current;
					triangles[i++] = next + 1;
					triangles[i++] = next;
				}
			}
			 
			//Bottom Cap
			for( var lon = 0; lon < p_longitudeCount; lon++ )
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