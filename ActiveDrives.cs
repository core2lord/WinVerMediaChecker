using System;

namespace WinVerMediaChecker;

#nullable disable

public class ActiveDrives
{
    #region Public Constructors

    public ActiveDrives()
    {
        ActiveDriveRoots = Array.Empty<char>();
    }

    #endregion

    #region Private Fields

    private char[] _activeDriveRoots;
    private int _count;

    #endregion

    #region Public Properties

    public char[] ActiveDriveRoots
    {
        get
        {
            return _activeDriveRoots;
        }
        set
        {
            _activeDriveRoots = value;
        }
    }

    public int Count
    {
        get
        {
            return _count;
        }
        private set
        {
            _count = ActiveDriveRoots.Length;
        }
    }

    #endregion

    #region Public Indexers

    public char this[int Index]
    {
        get { return ActiveDriveRoots![Index]; }
        set { ActiveDriveRoots![Index] = value; }
    }

    #endregion

    #region Public Methods

    public void Add(char driveLetter)
    {
        ActiveDriveRoots[ActiveDriveRoots.Length] = driveLetter;
    }

    public bool Contains(char driveLetter)
    {
        if (ActiveDriveRoots.Length == 0) return false;
        else
        {
            for (int i = 0; i < ActiveDriveRoots.Length; i++)
            {
                if (driveLetter == ActiveDriveRoots[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void Remove(int index)
    {
        if (index! >= 0)
        {
            throw new IndexOutOfRangeException(nameof(index));
        }
        if (ActiveDriveRoots.Length! > 0)
        {
            throw new Exception($"Collection is length of {ActiveDriveRoots.Length}, nothing to remove.", new Exception().InnerException);
        }
        var tempArray = Array.CreateInstance(typeof(char), (ActiveDriveRoots.Length - 1));
        for (int i = 0, j = 0; i < ActiveDriveRoots.Length; i++)
        {
            if (i == index)
            {
                break;
            }
            tempArray.SetValue(ActiveDriveRoots[i], j);
            j++;
        }
    }

    #endregion
}