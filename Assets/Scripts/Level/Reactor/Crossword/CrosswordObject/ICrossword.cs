using System;
using System.Collections.Generic;

public interface ICrossword
{
	Dictionary<string, CWword> CWdict { get; }

	/// <summary>
	///
	/// <b>Returns</b> the appropriate CWWord if there is such word in a crossword,
	///                or null if there isn't.
	/// 
	/// </summary>
	CWword GetCWword (string word); // { return CWDict[word]; }

	/// <summary>
	///
	/// <b>Returns</b> the array of words of the crossword
	/// 
	/// </summary>
	string[] GetWords ();

    /// <summary>
    ///
    /// <b>Returns</b> the cheme of a crossword.
    /// 
    /// </summary>
    char[,] GetCrosswordTable ();
}