// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PyramidInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC465420-9110-4296-A108-AF3AB2EFD84E
// Assembly location: C:\Users\bayba\OneDrive\Masaüstü\Source_v5.5_mintank\BussinesRe\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class PyramidInfo : DataObject
  {
    private int _ID;
    private int _userID;
    private int _currentLayer;
    private int _maxLayer;
    private int _totalPoint;
    private int _turnPoint;
    private int _pointRatio;
    private int _currentFreeCount;
    private bool _isPyramidStart;
    private string _LayerItems;
    private int _currentReviveCount;

    public int ID
    {
      get
      {
        return this._ID;
      }
      set
      {
        this._ID = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
        this._isDirty = true;
      }
    }

    public int currentLayer
    {
      get
      {
        return this._currentLayer;
      }
      set
      {
        this._currentLayer = value;
        this._isDirty = true;
      }
    }

    public int maxLayer
    {
      get
      {
        return this._maxLayer;
      }
      set
      {
        this._maxLayer = value;
        this._isDirty = true;
      }
    }

    public int totalPoint
    {
      get
      {
        return this._totalPoint;
      }
      set
      {
        this._totalPoint = value;
        this._isDirty = true;
      }
    }

    public int turnPoint
    {
      get
      {
        return this._turnPoint;
      }
      set
      {
        this._turnPoint = value;
        this._isDirty = true;
      }
    }

    public int pointRatio
    {
      get
      {
        return this._pointRatio;
      }
      set
      {
        this._pointRatio = value;
        this._isDirty = true;
      }
    }

    public int currentFreeCount
    {
      get
      {
        return this._currentFreeCount;
      }
      set
      {
        this._currentFreeCount = value;
        this._isDirty = true;
      }
    }

    public bool isPyramidStart
    {
      get
      {
        return this._isPyramidStart;
      }
      set
      {
        this._isPyramidStart = value;
        this._isDirty = true;
      }
    }

    public string LayerItems
    {
      get
      {
        return this._LayerItems;
      }
      set
      {
        this._LayerItems = value;
        this._isDirty = true;
      }
    }

    public int currentReviveCount
    {
      get
      {
        return this._currentReviveCount;
      }
      set
      {
        this._currentReviveCount = value;
        this._isDirty = true;
      }
    }
  }
}
