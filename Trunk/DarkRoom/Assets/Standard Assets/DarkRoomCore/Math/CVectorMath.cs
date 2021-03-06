using UnityEngine;
using System;

namespace DarkRoom.Core
{
    /// <summary>
    /// line是直线, segment是线段
    /// </summary>
	public static class CVectorMath {
		/** Returns the closest point on the line.
		 * The line is treated as infinite.
		 * \see ClosestPointOnSegment
		 * \see ClosestPointOnLineFactor
		 */
		public static Vector3 ClosestPointOnLine (Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
			float dot = Vector3.Dot(point - lineStart, lineDirection);

			return lineStart + (dot*lineDirection);
		}

		/** Factor along the line which is closest to the point.
		 * Returned value is in the range [0,1] if the point lies on the segment otherwise it just lies on the line.
		 * The closest point can be calculated using (end-start)*factor + start.
		 *
		 * \see ClosestPointOnLine
		 * \see ClosestPointOnSegment
		 */
		public static float ClosestPointOnLineFactor (Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			var dir = lineEnd - lineStart;
			float sqrMagn = dir.sqrMagnitude;

			if (sqrMagn <= 0.000001) return 0;

			return Vector3.Dot(point - lineStart, dir) / sqrMagn;
		}

		/** Returns the closest point on the segment.
		 * The segment is NOT treated as infinite.
		 * \see ClosestPointOnLine
		 * \see ClosestPointOnSegmentXZ
		 */
		public static Vector3 ClosestPointOnSegment (Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			var dir = lineEnd - lineStart;
			float sqrMagn = dir.sqrMagnitude;

			if (sqrMagn <= 0.000001) return lineStart;

			float factor = Vector3.Dot(point - lineStart, dir) / sqrMagn;
			return lineStart + Mathf.Clamp01(factor)*dir;
		}

		/** Returns the closest point on the segment in the XZ plane.
		 * The y coordinate of the result will be the same as the y coordinate of the \a point parameter.
		 *
		 * The segment is NOT treated as infinite.
		 * \see ClosestPointOnSegment
		 * \see ClosestPointOnLine
		 */
		public static Vector3 ClosestPointOnSegmentXZ (Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
			lineStart.y = point.y;
			lineEnd.y = point.y;
			Vector3 fullDirection = lineEnd-lineStart;
			Vector3 fullDirection2 = fullDirection;
			fullDirection2.y = 0;
			float magn = fullDirection2.magnitude;
			Vector3 lineDirection = magn > float.Epsilon ? fullDirection2/magn : Vector3.zero;

			float closestPoint = Vector3.Dot((point-lineStart), lineDirection);
			return lineStart+(Mathf.Clamp(closestPoint, 0.0f, fullDirection2.magnitude)*lineDirection);
		}

		/** Returns the approximate shortest squared distance between x,z and the segment p-q.
		 * The segment is not considered infinite.
		 * This function is not entirely exact, but it is about twice as fast as DistancePointSegment2.
		 * \todo Is this actually approximate? It looks exact.
		 */
		public static float SqrDistancePointSegmentApproximate (int x, int z, int px, int pz, int qx, int qz) {
			float pqx = (float)(qx - px);
			float pqz = (float)(qz - pz);
			float dx = (float)(x - px);
			float dz = (float)(z - pz);
			float d = pqx*pqx + pqz*pqz;
			float t = pqx*dx + pqz*dz;

			if (d > 0)
				t /= d;
			if (t < 0)
				t = 0;
			else if (t > 1)
				t = 1;

			dx = px + t*pqx - x;
			dz = pz + t*pqz - z;

			return dx*dx + dz*dz;
		}

		/** Normalize vector and also return the magnitude.
		 * This is more efficient than calculating the magnitude and normalizing separately
		 */
		public static Vector2 Normalize(Vector2 v, out float magnitude) {
			magnitude = v.magnitude;
			// This is the same constant that Unity uses
			if (magnitude > 1E-05f) {
				return v / magnitude;
			} else {
				return Vector2.zero;
			}
		}

		/** Returns the approximate shortest squared distance between x,z and the segment p-q.
		 * The segment is not considered infinite.
		 * This function is not entirely exact, but it is about twice as fast as DistancePointSegment2.
		 * \todo Is this actually approximate? It looks exact.
		 */
		public static float SqrDistancePointSegmentApproximate (Vector3Int a, Vector3Int b, Vector3Int p) {
			float pqx = (float)(b.x - a.x);
			float pqz = (float)(b.z - a.z);
			float dx = (float)(p.x - a.x);
			float dz = (float)(p.z - a.z);
			float d = pqx*pqx + pqz*pqz;
			float t = pqx*dx + pqz*dz;

			if (d > 0)
				t /= d;
			if (t < 0)
				t = 0;
			else if (t > 1)
				t = 1;

			dx = a.x + t*pqx - p.x;
			dz = a.z + t*pqz - p.z;

			return dx*dx + dz*dz;
		}

		/** Returns the squared distance between p and the segment a-b.
		 * The line is not considered infinite.
		 * ----------------------------------------------------------------------------------------------------
		 */
		public static float SqrDistancePointSegment (Vector3 a, Vector3 b, Vector3 p) {
			var nearest = ClosestPointOnSegment(a, b, p);

			return (nearest-p).sqrMagnitude;
		}

		/** 3D minimum distance between 2 segments.
		 * Input: two 3D line segments S1 and S2
		 * \returns the shortest squared distance between S1 and S2
		 */
		public static float SqrDistanceSegmentSegment (Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2) {
			Vector3 u = e1 - s1;
			Vector3 v = e2 - s2;
			Vector3 w = s1 - s2;
			float a = Vector3.Dot(u, u);           // always >= 0
			float b = Vector3.Dot(u, v);
			float c = Vector3.Dot(v, v);           // always >= 0
			float d = Vector3.Dot(u, w);
			float e = Vector3.Dot(v, w);
			float D = a*c - b*b;           // always >= 0
			float sc, sN, sD = D;          // sc = sN / sD, default sD = D >= 0
			float tc, tN, tD = D;          // tc = tN / tD, default tD = D >= 0

			// compute the line parameters of the two closest points
			if (D < 0.000001f) { // the lines are almost parallel
				sN = 0.0f;         // force using point P0 on segment S1
				sD = 1.0f;         // to prevent possible division by 0.0 later
				tN = e;
				tD = c;
			} else {               // get the closest points on the infinite lines
				sN = (b*e - c*d);
				tN = (a*e - b*d);
				if (sN < 0.0f) {        // sc < 0 => the s=0 edge is visible
					sN = 0.0f;
					tN = e;
					tD = c;
				} else if (sN > sD) { // sc > 1  => the s=1 edge is visible
					sN = sD;
					tN = e + b;
					tD = c;
				}
			}

			if (tN < 0.0f) {            // tc < 0 => the t=0 edge is visible
				tN = 0.0f;
				// recompute sc for this edge
				if (-d < 0.0f)
					sN = 0.0f;
				else if (-d > a)
					sN = sD;
				else {
					sN = -d;
					sD = a;
				}
			} else if (tN > tD) {    // tc > 1  => the t=1 edge is visible
				tN = tD;
				// recompute sc for this edge
				if ((-d + b) < 0.0f)
					sN = 0;
				else if ((-d + b) > a)
					sN = sD;
				else {
					sN = (-d +  b);
					sD = a;
				}
			}
			// finally do the division to get sc and tc
			sc = (Math.Abs(sN) < 0.000001f ? 0.0f : sN / sD);
			tc = (Math.Abs(tN) < 0.000001f ? 0.0f : tN / tD);

			// get the difference of the two closest points
			Vector3 dP = w + (sc * u) - (tc * v);  // =  S1(sc) - S2(tc)

			return dP.sqrMagnitude;   // return the closest distance
		}

		/** Squared distance between two points in the XZ plane */
		public static float SqrDistanceXZ (Vector3 a, Vector3 b) {
			var delta = a-b;

			return delta.x*delta.x+delta.z*delta.z;
		}

		/** Signed area of a triangle in the XZ plane multiplied by 2.
		 * This will be negative for clockwise triangles and positive for counter-clockwise ones
		 */
		public static long SignedTriangleAreaTimes2XZ (Vector3Int a, Vector3Int b, Vector3Int c) {
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);
		}

		/** Signed area of a triangle in the XZ plane multiplied by 2.
		 * This will be negative for clockwise triangles and positive for counter-clockwise ones.
		 */
		public static float SignedTriangleAreaTimes2XZ (Vector3 a, Vector3 b, Vector3 c) {
			return (b.x - a.x) * (c.z - a.z) - (c.x - a.x) * (b.z - a.z);
		}

		/** Returns if \a p lies on the right side of the line \a a - \a b.
		 * Uses XZ space. Does not return true if the points are colinear.
		 */
		public static bool RightXZ (Vector3 a, Vector3 b, Vector3 p) {
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) < -float.Epsilon;
		}

		/** Returns if \a p lies on the right side of the line \a a - \a b.
		 * Uses XZ space. Does not return true if the points are colinear.
		 */
		public static bool RightXZ (Vector3Int a, Vector3Int b, Vector3Int p) {
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) < 0;
		}

		/** Returns if \a p lies on the right side of the line \a a - \a b.
		 * Also returns true if the points are colinear.
		 */
		public static bool RightOrColinear (Vector2 a, Vector2 b, Vector2 p) {
			return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0;
		}

		/** Returns if \a p lies on the right side of the line \a a - \a b.
		 * Also returns true if the points are colinear.
		 */
		public static bool RightOrColinear (Vector2Int a, Vector2Int b, Vector2Int p) {
			return (long)(b.x - a.x) * (long)(p.y - a.y) - (long)(p.x - a.x) * (long)(b.y - a.y) <= 0;
		}

		/** Returns if \a p lies on the left side of the line \a a - \a b.
		 * Uses XZ space. Also returns true if the points are colinear.
		 */
		public static bool RightOrColinearXZ (Vector3 a, Vector3 b, Vector3 p) {
			return (b.x - a.x) * (p.z - a.z) - (p.x - a.x) * (b.z - a.z) <= 0;
		}

		/** Returns if \a p lies on the left side of the line \a a - \a b.
		 * Uses XZ space. Also returns true if the points are colinear.
		 */
		public static bool RightOrColinearXZ (Vector3Int a, Vector3Int b, Vector3Int p) {
			return (long)(b.x - a.x) * (long)(p.z - a.z) - (long)(p.x - a.x) * (long)(b.z - a.z) <= 0;
		}

		/** Returns if the points a in a clockwise order.
		 * Will return true even if the points are colinear or very slightly counter-clockwise
		 * (if the signed area of the triangle formed by the points has an area less than or equals to float.Epsilon) */
		public static bool IsClockwiseMarginXZ (Vector3 a, Vector3 b, Vector3 c) {
			return (b.x-a.x)*(c.z-a.z)-(c.x-a.x)*(b.z-a.z) <= float.Epsilon;
		}

		/** Returns if the points a in a clockwise order */
		public static bool IsClockwiseXZ (Vector3 a, Vector3 b, Vector3 c) {
			return (b.x-a.x)*(c.z-a.z)-(c.x-a.x)*(b.z-a.z) < 0;
		}

		/** Returns if the points a in a clockwise order */
		public static bool IsClockwiseXZ (Vector3Int a, Vector3Int b, Vector3Int c) {
			return RightXZ(a, b, c);
		}

		/** Returns true if the points a in a clockwise order or if they are colinear */
		public static bool IsClockwiseOrColinearXZ (Vector3Int a, Vector3Int b, Vector3Int c) {
			return RightOrColinearXZ(a, b, c);
		}

		/** Returns true if the points a in a clockwise order or if they are colinear */
		public static bool IsClockwiseOrColinear (Vector2Int a, Vector2Int b, Vector2Int c) {
			return RightOrColinear(a, b, c);
		}

		/** Returns if the points are colinear (lie on a straight line) */
		public static bool IsColinearXZ (Vector3Int a, Vector3Int b, Vector3Int c) {
			return (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z) == 0;
		}

		/** Returns if the points are colinear (lie on a straight line) */
		public static bool IsColinearXZ (Vector3 a, Vector3 b, Vector3 c) {
			float v = (b.x-a.x)*(c.z-a.z)-(c.x-a.x)*(b.z-a.z);

			// Epsilon not chosen with much though, just that float.Epsilon was a bit too small.
			return v <= 0.0000001f && v >= -0.0000001f;
		}

		/** Returns if the points are colinear (lie on a straight line) */
		public static bool IsColinearAlmostXZ (Vector3Int a, Vector3Int b, Vector3Int c) {
			long v = (long)(b.x - a.x) * (long)(c.z - a.z) - (long)(c.x - a.x) * (long)(b.z - a.z);

			return v > -1 && v < 1;
		}

		/** Returns if the line segment \a start2 - \a end2 intersects the line segment \a start1 - \a end1.
		 * If only the endpoints coincide, the result is undefined (may be true or false).
		 */
		public static bool SegmentsIntersect (Vector2Int start1, Vector2Int end1, Vector2Int start2, Vector2Int end2) {
			return RightOrColinear(start1, end1, start2) != RightOrColinear(start1, end1, end2) && RightOrColinear(start2, end2, start1) != RightOrColinear(start2, end2, end1);
		}

		/** Returns if the line segment \a start2 - \a end2 intersects the line segment \a start1 - \a end1.
		 * If only the endpoints coincide, the result is undefined (may be true or false).
		 *
		 * \note XZ space
		 */
		public static bool SegmentsIntersectXZ (Vector3Int start1, Vector3Int end1, Vector3Int start2, Vector3Int end2) {
			return RightOrColinearXZ(start1, end1, start2) != RightOrColinearXZ(start1, end1, end2) && RightOrColinearXZ(start2, end2, start1) != RightOrColinearXZ(start2, end2, end1);
		}

		/** Returns if the two line segments intersects. The lines are NOT treated as infinite (just for clarification)
		 * \see IntersectionPoint
		 */
		public static bool SegmentsIntersectXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2) {
			Vector3 dir1 = end1-start1;
			Vector3 dir2 = end2-start2;

			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				return false;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float nom2 = dir1.x*(start1.z-start2.z) - dir1.z * (start1.x - start2.x);
			float u = nom/den;
			float u2 = nom2/den;

			if (u < 0F || u > 1F || u2 < 0F || u2 > 1F) {
				return false;
			}

			return true;
		}

		/** Intersection point between two infinite lines.
		 * Note that start points and directions are taken as parameters instead of start and end points.
		 * Lines are treated as infinite. If the lines are parallel 'start1' will be returned.
		 * Intersections are calculated on the XZ plane.
		 *
		 * \see LineIntersectionPointXZ
		 */
		public static Vector3 LineDirIntersectionPointXZ (Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2) {
			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				return start1;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float u = nom/den;

			return start1 + dir1*u;
		}

		/** Intersection point between two infinite lines.
		 * Note that start points and directions are taken as parameters instead of start and end points.
		 * Lines are treated as infinite. If the lines are parallel 'start1' will be returned.
		 * Intersections are calculated on the XZ plane.
		 *
		 * \see LineIntersectionPointXZ
		 */
		public static Vector3 LineDirIntersectionPointXZ (Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2, out bool intersects) {
			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				intersects = false;
				return start1;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float u = nom/den;

			intersects = true;
			return start1 + dir1*u;
		}

		/** Returns if the ray (start1, end1) intersects the segment (start2, end2).
		 * false is returned if the lines are parallel.
		 * Only the XZ coordinates are used.
		 * \todo Double check that this actually works
		 */
		public static bool RaySegmentIntersectXZ (Vector3Int start1, Vector3Int end1, Vector3Int start2, Vector3Int end2) {
			Vector3Int dir1 = end1-start1;
			Vector3Int dir2 = end2-start2;

			long den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				return false;
			}

			long nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			long nom2 = dir1.x*(start1.z-start2.z) - dir1.z * (start1.x - start2.x);

			//factor1 < 0
			// If both have the same sign, then nom/den < 0 and thus the segment cuts the ray before the ray starts
			if (!(nom < 0 ^ den < 0)) {
				return false;
			}

			//factor2 < 0
			if (!(nom2 < 0 ^ den < 0)) {
				return false;
			}

			if ((den >= 0 && nom2 > den) || (den < 0 && nom2 <= den)) {
				return false;
			}

			return true;
		}

		/** Returns the intersection factors for line 1 and line 2. The intersection factors is a distance along the line \a start - \a end where the other line intersects it.\n
		 * \code intersectionPoint = start1 + factor1 * (end1-start1) \endcode
		 * \code intersectionPoint2 = start2 + factor2 * (end2-start2) \endcode
		 * Lines are treated as infinite.\n
		 * false is returned if the lines are parallel and true if they are not.
		 * Only the XZ coordinates are used.
		 */
		public static bool LineIntersectionFactorXZ (Vector3Int start1, Vector3Int end1, Vector3Int start2, Vector3Int end2, out float factor1, out float factor2) {
			Vector3Int dir1 = end1-start1;
			Vector3Int dir2 = end2-start2;

			long den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				factor1 = 0;
				factor2 = 0;
				return false;
			}

			long nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			long nom2 = dir1.x*(start1.z-start2.z) - dir1.z * (start1.x - start2.x);

			factor1 = (float)nom/den;
			factor2 = (float)nom2/den;

			return true;
		}

		/** Returns the intersection factors for line 1 and line 2. The intersection factors is a distance along the line \a start - \a end where the other line intersects it.\n
		 * \code intersectionPoint = start1 + factor1 * (end1-start1) \endcode
		 * \code intersectionPoint2 = start2 + factor2 * (end2-start2) \endcode
		 * Lines are treated as infinite.\n
		 * false is returned if the lines are parallel and true if they are not.
		 * Only the XZ coordinates are used.
		 */
		public static bool LineIntersectionFactorXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out float factor1, out float factor2) {
			Vector3 dir1 = end1-start1;
			Vector3 dir2 = end2-start2;

			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den <= 0.00001f && den >= -0.00001f) {
				factor1 = 0;
				factor2 = 0;
				return false;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float nom2 = dir1.x*(start1.z-start2.z) - dir1.z * (start1.x - start2.x);

			float u = nom/den;
			float u2 = nom2/den;

			factor1 = u;
			factor2 = u2;

			return true;
		}

		/** Returns the intersection factor for line 1 with ray 2.
		 * The intersection factors is a factor distance along the line \a start - \a end where the other line intersects it.\n
		 * \code intersectionPoint = start1 + factor * (end1-start1) \endcode
		 * Lines are treated as infinite.\n
		 *
		 * The second "line" is treated as a ray, meaning only matches on start2 or forwards towards end2 (and beyond) will be returned
		 * If the point lies on the wrong side of the ray start, Nan will be returned.
		 *
		 * NaN is returned if the lines are parallel. */
		public static float LineRayIntersectionFactorXZ (Vector3Int start1, Vector3Int end1, Vector3Int start2, Vector3Int end2) {
			Vector3Int dir1 = end1-start1;
			Vector3Int dir2 = end2-start2;

			int den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				return float.NaN;
			}

			int nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			int nom2 = dir1.x*(start1.z-start2.z) - dir1.z * (start1.x - start2.x);

			if ((float)nom2/den < 0) {
				return float.NaN;
			}
			return (float)nom/den;
		}

		/** Returns the intersection factor for line 1 with line 2.
		 * The intersection factor is a distance along the line \a start1 - \a end1 where the line \a start2 - \a end2 intersects it.\n
		 * \code intersectionPoint = start1 + intersectionFactor * (end1-start1) \endcode.
		 * Lines are treated as infinite.\n
		 * -1 is returned if the lines are parallel (note that this is a valid return value if they are not parallel too) */
		public static float LineIntersectionFactorXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2) {
			Vector3 dir1 = end1-start1;
			Vector3 dir2 = end2-start2;

			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				return -1;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float u = nom/den;

			return u;
		}

		/** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
		public static Vector3 LineIntersectionPointXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2) {
			bool s;

			return LineIntersectionPointXZ(start1, end1, start2, end2, out s);
		}

		/** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
		public static Vector3 LineIntersectionPointXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects) {
			Vector3 dir1 = end1-start1;
			Vector3 dir2 = end2-start2;

			float den = dir2.z*dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				intersects = false;
				return start1;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);

			float u = nom/den;

			intersects = true;
			return start1 + dir1*u;
		}

		/** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
		public static Vector2 LineIntersectionPoint (Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2) {
			bool s;

			return LineIntersectionPoint(start1, end1, start2, end2, out s);
		}

		/** Returns the intersection point between the two lines. Lines are treated as infinite. \a start1 is returned if the lines are parallel */
		public static Vector2 LineIntersectionPoint (Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out bool intersects) {
			Vector2 dir1 = end1-start1;
			Vector2 dir2 = end2-start2;

			float den = dir2.y*dir1.x - dir2.x * dir1.y;

			if (den == 0) {
				intersects = false;
				return start1;
			}

			float nom = dir2.x*(start1.y-start2.y)- dir2.y*(start1.x-start2.x);

			float u = nom/den;

			intersects = true;
			return start1 + dir1*u;
		}

		/** Returns the intersection point between the two line segments in XZ space.
		 * Lines are NOT treated as infinite. \a start1 is returned if the line segments do not intersect
		 * The point will be returned along the line [start1, end1] (this matters only for the y coordinate).
		 */
		public static Vector3 SegmentIntersectionPointXZ (Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects) {
			Vector3 dir1 = end1-start1;
			Vector3 dir2 = end2-start2;

			float den = dir2.z * dir1.x - dir2.x * dir1.z;

			if (den == 0) {
				intersects = false;
				return start1;
			}

			float nom = dir2.x*(start1.z-start2.z)- dir2.z*(start1.x-start2.x);
			float nom2 = dir1.x*(start1.z-start2.z) - dir1.z*(start1.x-start2.x);
			float u = nom/den;
			float u2 = nom2/den;

			if (u < 0F || u > 1F || u2 < 0F || u2 > 1F) {
				intersects = false;
				return start1;
			}

			intersects = true;
			return start1 + dir1*u;
		}

		/** Does the line segment intersect the bounding box.
		 * The line is NOT treated as infinite.
		 * \author Slightly modified code from http://www.3dkingdoms.com/weekly/weekly.php?a=21
		 */
		public static bool SegmentIntersectsBounds (Bounds bounds, Vector3 a, Vector3 b) {
			// Put segment in box space
			a -= bounds.center;
			b -= bounds.center;

			// Get line midpoint and extent
			var LMid = (a + b) * 0.5F;
			var L = (a - LMid);
			var LExt = new Vector3(Math.Abs(L.x), Math.Abs(L.y), Math.Abs(L.z));

			Vector3 extent = bounds.extents;

			// Use Separating Axis Test
			// Separation vector from box center to segment center is LMid, since the line is in box space
			if (Math.Abs(LMid.x) > extent.x + LExt.x) return false;
			if (Math.Abs(LMid.y) > extent.y + LExt.y) return false;
			if (Math.Abs(LMid.z) > extent.z + LExt.z) return false;
			// Crossproducts of line and each axis
			if (Math.Abs(LMid.y * L.z - LMid.z * L.y) > (extent.y * LExt.z + extent.z * LExt.y)) return false;
			if (Math.Abs(LMid.x * L.z - LMid.z * L.x) > (extent.x * LExt.z + extent.z * LExt.x)) return false;
			if (Math.Abs(LMid.x * L.y - LMid.y * L.x) > (extent.x * LExt.y + extent.y * LExt.x)) return false;
			// No separating axis, the line intersects
			return true;
		}

		/** True if the matrix will reverse orientations of faces.
		 *
		 * Scaling by a negative value along an odd number of axes will reverse
		 * the orientation of e.g faces on a mesh. This must be counter adjusted
		 * by for example the recast rasterization system to be able to handle
		 * meshes with negative scales properly.
		 *
		 * We can find out if they are flipped by finding out how the signed
		 * volume of a unit cube is transformed when applying the matrix
		 *
		 * If the (signed) volume turns out to be negative
		 * that also means that the orientation of it has been reversed.
		 *
		 * \see https://en.wikipedia.org/wiki/Normal_(geometry)
		 * \see https://en.wikipedia.org/wiki/Parallelepiped
		 */
		public static bool ReversesFaceOrientations (Matrix4x4 matrix) {
			var dX = matrix.MultiplyVector(new Vector3(1, 0, 0));
			var dY = matrix.MultiplyVector(new Vector3(0, 1, 0));
			var dZ = matrix.MultiplyVector(new Vector3(0, 0, 1));

			// Calculate the signed volume of the parallelepiped
			var volume = Vector3.Dot(Vector3.Cross(dX, dY), dZ);

			return volume < 0;
		}

		/** True if the matrix will reverse orientations of faces in the XZ plane.
		 * Almost the same as ReversesFaceOrientations, but this method assumes
		 * that scaling a face with a negative scale along the Y axis does not
		 * reverse the orientation of the face.
		 *
		 * This is used for navmesh cuts.
		 *
		 * Scaling by a negative value along one axis or rotating
		 * it so that it is upside down will reverse
		 * the orientation of the cut, so we need to be reverse
		 * it again as a countermeasure.
		 * However if it is flipped along two axes it does not need to
		 * be reversed.
		 * We can handle all these cases by finding out how a unit square formed
		 * by our forward axis and our rightward axis is transformed in XZ space
		 * when applying the local to world matrix.
		 * If the (signed) area of the unit square turns out to be negative
		 * that also means that the orientation of it has been reversed.
		 * The signed area is calculated using a cross product of the vectors.
		 */
		public static bool ReversesFaceOrientationsXZ (Matrix4x4 matrix) {
			var dX = matrix.MultiplyVector(new Vector3(1, 0, 0));
			var dZ = matrix.MultiplyVector(new Vector3(0, 0, 1));

			// Take the cross product of the vectors projected onto the XZ plane
			var cross = (dX.x*dZ.z - dZ.x*dX.z);

			return cross < 0;
		}
	}

	/** Utility functions for working with polygons, lines, and other vector math.
	 * All functions which accepts Vector3s but work in 2D space uses the XZ space if nothing else is said.
	 *
	 * \version A lot of functions in this class have been moved to the VectorMath class
	 * the names have changed slightly and everything now consistently assumes a left handed
	 * coordinate system now instead of sometimes using a left handed one and sometimes
	 * using a right handed one. This is why the 'Left' methods redirect to methods
	 * named 'Right'. The functionality is exactly the same.
	 *
	 * \ingroup utils
	 */
	public static class Polygon {
		/** Returns if the triangle \a ABC contains the point \a p in XZ space.
		 * The triangle vertices are assumed to be laid out in clockwise order.
		 */
		public static bool ContainsPointXZ (Vector3 a, Vector3 b, Vector3 c, Vector3 p) {
			return CVectorMath.IsClockwiseMarginXZ(a, b, p) && CVectorMath.IsClockwiseMarginXZ(b, c, p) && CVectorMath.IsClockwiseMarginXZ(c, a, p);
		}
		
		/** Returns if the triangle \a ABC contains the point \a p.
		 * The triangle vertices are assumed to be laid out in clockwise order.
		 */
		public static bool ContainsPointXZ (Vector3Int a, Vector3Int b, Vector3Int c, Vector3Int p) {
			return CVectorMath.IsClockwiseOrColinearXZ(a, b, p) && CVectorMath.IsClockwiseOrColinearXZ(b, c, p) && CVectorMath.IsClockwiseOrColinearXZ(c, a, p);
		}

		/** Returns if the triangle \a ABC contains the point \a p.
		 * The triangle vertices are assumed to be laid out in clockwise order.
		 */
		public static bool ContainsPoint (Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int p) {
			return CVectorMath.IsClockwiseOrColinear(a, b, p) && CVectorMath.IsClockwiseOrColinear(b, c, p) && CVectorMath.IsClockwiseOrColinear(c, a, p);
		}

		/** Checks if \a p is inside the polygon.
		 * \author http://unifycommunity.com/wiki/index.php?title=PolyContainsPoint (Eric5h5)
		 */
		public static bool ContainsPoint (Vector2[] polyPoints, Vector2 p) {
			int j = polyPoints.Length-1;
			bool inside = false;

			for (int i = 0; i < polyPoints.Length; j = i++) {
				if (((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) &&
					(p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
					inside = !inside;
			}
			return inside;
		}

		/** Checks if \a p is inside the polygon (XZ space).
		 * \author http://unifycommunity.com/wiki/index.php?title=PolyContainsPoint (Eric5h5)
		 */
		public static bool ContainsPointXZ (Vector3[] polyPoints, Vector3 p) {
			int j = polyPoints.Length-1;
			bool inside = false;

			for (int i = 0; i < polyPoints.Length; j = i++) {
				if (((polyPoints[i].z <= p.z && p.z < polyPoints[j].z) || (polyPoints[j].z <= p.z && p.z < polyPoints[i].z)) &&
					(p.x < (polyPoints[j].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[j].z - polyPoints[i].z) + polyPoints[i].x))
					inside = !inside;
			}
			return inside;
		}


		/** Subdivides \a path and returns the new array with interpolated values.
		 * The returned array is \a path subdivided \a subdivisions times, the resulting points are interpolated using Mathf.SmoothStep.\n
		 * If \a subdivisions is less or equal to 0 (zero), the original array will be returned */
		public static Vector3[] Subdivide (Vector3[] path, int subdivisions) {
			subdivisions = subdivisions < 0 ? 0 : subdivisions;

			if (subdivisions == 0) {
				return path;
			}

			var path2 = new Vector3[(path.Length-1)*(int)Mathf.Pow(2, subdivisions)+1];

			int c = 0;
			for (int p = 0; p < path.Length-1; p++) {
				float step = 1.0F/Mathf.Pow(2, subdivisions);

				for (float i = 0; i < 1.0F; i += step) {
					path2[c] = Vector3.Lerp(path[p], path[p+1], Mathf.SmoothStep(0, 1, i));
					c++;
				}
			}

			path2[c] = path[path.Length-1];
			return path2;
		}

		/** Closest point on the triangle abc to the point p.
		 * \see 'Real Time Collision Detection' by Christer Ericson, chapter 5.1, page 141
		 */
		public static Vector2 ClosestPointOnTriangle (Vector2 a, Vector2 b, Vector2 c, Vector2 p) {
			// Check if p is in vertex region outside A
			var ab = b - a;
			var ac = c - a;
			var ap = p - a;

			var d1 = Vector2.Dot(ab, ap);
			var d2 = Vector2.Dot(ac, ap);

			// Barycentric coordinates (1,0,0)
			if (d1 <= 0 && d2 <= 0) {
				return a;
			}

			// Check if p is in vertex region outside B
			var bp = p - b;
			var d3 = Vector2.Dot(ab, bp);
			var d4 = Vector2.Dot(ac, bp);

			// Barycentric coordinates (0,1,0)
			if (d3 >= 0 && d4 <= d3) {
				return b;
			}

			// Check if p is in edge region outside AB, if so return a projection of p onto AB
			if (d1 >= 0 && d3 <= 0) {
				var vc = d1 * d4 - d3 * d2;
				if (vc <= 0) {
					// Barycentric coordinates (1-v, v, 0)
					var v = d1 / (d1 - d3);
					return a + ab*v;
				}
			}

			// Check if p is in vertex region outside C
			var cp = p - c;
			var d5 = Vector2.Dot(ab, cp);
			var d6 = Vector2.Dot(ac, cp);

			// Barycentric coordinates (0,0,1)
			if (d6 >= 0 && d5 <= d6) {
				return c;
			}

			// Check if p is in edge region of AC, if so return a projection of p onto AC
			if (d2 >= 0 && d6 <= 0) {
				var vb = d5 * d2 - d1 * d6;
				if (vb <= 0) {
					// Barycentric coordinates (1-v, 0, v)
					var v = d2 / (d2 - d6);
					return a + ac*v;
				}
			}

			// Check if p is in edge region of BC, if so return projection of p onto BC
			if ((d4 - d3) >= 0 && (d5 - d6) >= 0) {
				var va = d3 * d6 - d5 * d4;
				if (va <= 0) {
					var v = (d4 - d3) / ((d4 - d3) + (d5 - d6));
					return b + (c - b) * v;
				}
			}

			return p;
		}

		/** Closest point on the triangle abc to the point p.
		 * \see 'Real Time Collision Detection' by Christer Ericson, chapter 5.1, page 141
		 */
		public static Vector3 ClosestPointOnTriangle (Vector3 a, Vector3 b, Vector3 c, Vector3 p) {
			// Check if p is in vertex region outside A
			var ab = b - a;
			var ac = c - a;
			var ap = p - a;

			var d1 = Vector3.Dot(ab, ap);
			var d2 = Vector3.Dot(ac, ap);

			// Barycentric coordinates (1,0,0)
			if (d1 <= 0 && d2 <= 0)
				return a;

			// Check if p is in vertex region outside B
			var bp = p - b;
			var d3 = Vector3.Dot(ab, bp);
			var d4 = Vector3.Dot(ac, bp);

			// Barycentric coordinates (0,1,0)
			if (d3 >= 0 && d4 <= d3)
				return b;

			// Check if p is in edge region outside AB, if so return a projection of p onto AB
			var vc = d1 * d4 - d3 * d2;
			if (d1 >= 0 && d3 <= 0 && vc <= 0) {
				// Barycentric coordinates (1-v, v, 0)
				var v = d1 / (d1 - d3);
				return a + ab * v;
			}

			// Check if p is in vertex region outside C
			var cp = p - c;
			var d5 = Vector3.Dot(ab, cp);
			var d6 = Vector3.Dot(ac, cp);

			// Barycentric coordinates (0,0,1)
			if (d6 >= 0 && d5 <= d6)
				return c;

			// Check if p is in edge region of AC, if so return a projection of p onto AC
			var vb = d5 * d2 - d1 * d6;
			if (d2 >= 0 && d6 <= 0 && vb <= 0) {
				// Barycentric coordinates (1-v, 0, v)
				var v = d2 / (d2 - d6);
				return a + ac * v;
			}

			// Check if p is in edge region of BC, if so return projection of p onto BC
			var va = d3 * d6 - d5 * d4;
			if ((d4 - d3) >= 0 && (d5 - d6) >= 0 && va <= 0) {
				var v = (d4 - d3) / ((d4 - d3) + (d5 - d6));
				return b + (c - b) * v;
			} else {
				// P is inside the face region. Compute the point using its barycentric coordinates (u, v, w)
				var denom = 1f / (va + vb + vc);
				var v = vb * denom;
				var w = vc * denom;

				// This is equal to: u*a + v*b + w*c, u = va*denom = 1 - v - w;
				return a + ab * v + ac * w;
			}
		}
	}
}
