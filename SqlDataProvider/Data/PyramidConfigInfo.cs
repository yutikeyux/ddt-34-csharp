// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PyramidConfigInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BC465420-9110-4296-A108-AF3AB2EFD84E
// Assembly location: C:\Users\bayba\OneDrive\Masaüstü\Source_v5.5_mintank\BussinesRe\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class PyramidConfigInfo
  {
    private int _userID;
    private bool _isOpen;
    private bool _isScoreExchange;
    private DateTime _beginTime;
    private DateTime _endTime;
    private int _freeCount;
    private int _turnCardPrice;
    private int[] _revivePrice;

    public int UserID
    {
      get
      {
        return this._userID;
      }
      set
      {
        this._userID = value;
      }
    }

    public bool isOpen
    {
      get
      {
        return this._isOpen;
      }
      set
      {
        this._isOpen = value;
      }
    }

    public bool isScoreExchange
    {
      get
      {
        return this._isScoreExchange;
      }
      set
      {
        this._isScoreExchange = value;
      }
    }

    public DateTime beginTime
    {
      get
      {
        return this._beginTime;
      }
      set
      {
        this._beginTime = value;
      }
    }

    public DateTime endTime
    {
      get
      {
        return this._endTime;
      }
      set
      {
        this._endTime = value;
      }
    }

    public int freeCount
    {
      get
      {
        return this._freeCount;
      }
      set
      {
        this._freeCount = value;
      }
    }

    public int turnCardPrice
    {
      get
      {
        return this._turnCardPrice;
      }
      set
      {
        this._turnCardPrice = value;
      }
    }

    public int[] revivePrice
    {
      get
      {
        return this._revivePrice;
      }
      set
      {
        this._revivePrice = value;
      }
    }
  }
}
