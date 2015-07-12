using System;

namespace SoundLibrary.Music
{
	/// <summary>
	/// ïΩãœó•âπäKíËêîíËã`ÅB
	/// </summary>
	public class EqualTemperament
	{
		// é¸îgêîî‰
		public static readonly double TONE     = Math.Pow(2.0, 1/6.0);
		public static readonly double SEMITONE = Math.Pow(2.0, 1/12.0);
		public static readonly double SHARP  = SEMITONE;
		public static readonly double FLAT   = 1.0 / SEMITONE;
		public const double OCTAVE = 2.0;

		// âπäK
		public const double A3 = 440.0;
		public static readonly double B3 = A3 * TONE;
		public static readonly double C3 = B3 * SEMITONE / OCTAVE;
		public static readonly double D3 = C3 * TONE;
		public static readonly double E3 = D3 * TONE;
		public static readonly double F3 = E3 * SEMITONE;
		public static readonly double G3 = F3 * TONE;

		static readonly double[] Aarray = new double[]{
			A3 / OCTAVE / OCTAVE / OCTAVE, A3 / OCTAVE / OCTAVE,
			A3 / OCTAVE, A3, A3 * OCTAVE,
			A3 * OCTAVE * OCTAVE, A3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Barray = new double[]{
			B3 / OCTAVE / OCTAVE / OCTAVE, B3 / OCTAVE / OCTAVE,
			B3 / OCTAVE, B3, B3 * OCTAVE,
			B3 * OCTAVE * OCTAVE, B3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Carray = new double[]{
			C3 / OCTAVE / OCTAVE / OCTAVE, C3 / OCTAVE / OCTAVE,
			C3 / OCTAVE, C3, C3 * OCTAVE,
			C3 * OCTAVE * OCTAVE, C3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Darray = new double[]{
			D3 / OCTAVE / OCTAVE / OCTAVE, D3 / OCTAVE / OCTAVE,
			D3 / OCTAVE, D3, D3 * OCTAVE,
			D3 * OCTAVE * OCTAVE, D3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Earray = new double[]{
			E3 / OCTAVE / OCTAVE / OCTAVE, E3 / OCTAVE / OCTAVE,
			E3 / OCTAVE, E3, E3 * OCTAVE,
			E3 * OCTAVE * OCTAVE, E3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Farray = new double[]{
			F3 / OCTAVE / OCTAVE / OCTAVE, F3 / OCTAVE / OCTAVE,
			F3 / OCTAVE, F3, F3 * OCTAVE,
			F3 * OCTAVE * OCTAVE, F3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Garray = new double[]{
			G3 / OCTAVE / OCTAVE / OCTAVE, G3 / OCTAVE / OCTAVE,
			G3 / OCTAVE, G3, G3 * OCTAVE,
			G3 * OCTAVE * OCTAVE, G3 * OCTAVE * OCTAVE * OCTAVE};

		public static readonly double DO  = C3;
		public static readonly double RE  = D3;
		public static readonly double MI  = E3;
		public static readonly double FA  = F3;
		public static readonly double SOL = G3;
		public static readonly double LA  = A3;
		public static readonly double SI  = B3;

		public static double A(int i){return Aarray[i];}
		public static double B(int i){return Barray[i];}
		public static double C(int i){return Carray[i];}
		public static double D(int i){return Darray[i];}
		public static double E(int i){return Earray[i];}
		public static double F(int i){return Farray[i];}
		public static double G(int i){return Garray[i];}
	}//class EqualTemperament

	/// <summary>
	/// èÉêàó•âπäKíËêîíËã`ÅB
	/// </summary>
	public class PureTemperament
	{
		// é¸îgêîî‰
		public static readonly double TONE     = Math.Pow(2.0, 1/6.0);
		public static readonly double SEMITONE = Math.Pow(2.0, 1/12.0);
		public static readonly double SHARP  = SEMITONE;
		public static readonly double FLAT   = 1.0 / SEMITONE;
		public const double OCTAVE = 2.0;

		public const double MINOR2   = 10.0 / 9.0; // íZ2ìx
		public const double MAJOR2   =  9.0 / 8.0; // í∑2ìx
		public const double MINOR3   =  6.0 / 5.0; // íZ3ìx
		public const double MAJOR3   =  5.0 / 4.0; // í∑3ìx
		public const double PERFECT4 =  4.0 / 3.0; // äÆëS4ìx
		public const double PERFECT5 =  3.0 / 2.0; // äÆëS5ìx
		public const double MINOR6   =  8.0 / 5.0; // íZ6ìx
		public const double MAJOR6   =  5.0 / 3.0; // í∑6ìx
		public const double MINOR7   =  9.0 / 5.0; // íZ7ìx
		public const double MAJOR7   = 15.0 / 8.0; // í∑7ìx

		// âπäK
		public const double A3 = 440.0;
		public static readonly double B3 = A3 * MAJOR2;
		public static readonly double C3 = A3 * MAJOR3   / OCTAVE;
		public static readonly double D3 = A3 * PERFECT4 / OCTAVE;
		public static readonly double E3 = A3 * PERFECT5 / OCTAVE;
		public static readonly double F3 = A3 * MAJOR6   / OCTAVE;
		public static readonly double G3 = A3 * MAJOR7   / OCTAVE;

		static readonly double[] Aarray = new double[]{
			A3 / OCTAVE / OCTAVE / OCTAVE, A3 / OCTAVE / OCTAVE,
			A3 / OCTAVE, A3, A3 * OCTAVE,
			A3 * OCTAVE * OCTAVE, A3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Barray = new double[]{
			B3 / OCTAVE / OCTAVE / OCTAVE, B3 / OCTAVE / OCTAVE,
			B3 / OCTAVE, B3, B3 * OCTAVE,
			B3 * OCTAVE * OCTAVE, B3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Carray = new double[]{
			C3 / OCTAVE / OCTAVE / OCTAVE, C3 / OCTAVE / OCTAVE,
			C3 / OCTAVE, C3, C3 * OCTAVE,
			C3 * OCTAVE * OCTAVE, C3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Darray = new double[]{
			D3 / OCTAVE / OCTAVE / OCTAVE, D3 / OCTAVE / OCTAVE,
			D3 / OCTAVE, D3, D3 * OCTAVE,
			D3 * OCTAVE * OCTAVE, D3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Earray = new double[]{
			E3 / OCTAVE / OCTAVE / OCTAVE, E3 / OCTAVE / OCTAVE,
			E3 / OCTAVE, E3, E3 * OCTAVE,
			E3 * OCTAVE * OCTAVE, E3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Farray = new double[]{
			F3 / OCTAVE / OCTAVE / OCTAVE, F3 / OCTAVE / OCTAVE,
			F3 / OCTAVE, F3, F3 * OCTAVE,
			F3 * OCTAVE * OCTAVE, F3 * OCTAVE * OCTAVE * OCTAVE};
		static readonly double[] Garray = new double[]{
			G3 / OCTAVE / OCTAVE / OCTAVE, G3 / OCTAVE / OCTAVE,
			G3 / OCTAVE, G3, G3 * OCTAVE,
			G3 * OCTAVE * OCTAVE, G3 * OCTAVE * OCTAVE * OCTAVE};

		public static readonly double DO  = C3;
		public static readonly double RE  = D3;
		public static readonly double MI  = E3;
		public static readonly double FA  = F3;
		public static readonly double SOL = G3;
		public static readonly double LA  = A3;
		public static readonly double SI  = B3;

		public static double A(int i){return Aarray[i];}
		public static double B(int i){return Barray[i];}
		public static double C(int i){return Carray[i];}
		public static double D(int i){return Darray[i];}
		public static double E(int i){return Earray[i];}
		public static double F(int i){return Farray[i];}
		public static double G(int i){return Garray[i];}
	}//class PureTemperament
}//namespace Wave
