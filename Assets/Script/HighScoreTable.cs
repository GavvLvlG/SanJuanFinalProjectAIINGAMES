using System;
using System.Collections.Generic;
using UnityEngine;

public static class HighScoreTable
{
    private const string PlayerPrefsKey = "HighScores";
    private const int MaximumEntries = 10;

    [Serializable]
    public sealed class Entry
    {
        public string playerName;
        public int score;
    }

    [Serializable]
    private sealed class EntryCollection
    {
        public List<Entry> entries = new List<Entry>();
    }

    /// <summary>Loads the saved high scores in descending score order.</summary>
    public static List<Entry> LoadEntries()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey))
            return new List<Entry>();

        EntryCollection collection = JsonUtility.FromJson<EntryCollection>(PlayerPrefs.GetString(PlayerPrefsKey));
        if (collection == null || collection.entries == null)
            return new List<Entry>();

        collection.entries.RemoveAll(entry => entry == null || entry.score < 0);
        collection.entries.Sort((first, second) => second.score.CompareTo(first.score));

        if (collection.entries.Count > MaximumEntries)
            collection.entries.RemoveRange(MaximumEntries, collection.entries.Count - MaximumEntries);

        return collection.entries;
    }

    /// <summary>Adds a positive score when it qualifies for the saved top ten.</summary>
    public static void AddScore(string playerName, int score)
    {
        if (score <= 0)
            return;

        List<Entry> entries = LoadEntries();
        Entry newEntry = new Entry
        {
            playerName = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName.Trim(),
            score = score
        };

        int insertionIndex = 0;
        while (insertionIndex < entries.Count && entries[insertionIndex].score >= score)
            insertionIndex++;

        if (insertionIndex >= MaximumEntries)
            return;

        entries.Insert(insertionIndex, newEntry);
        if (entries.Count > MaximumEntries)
            entries.RemoveAt(entries.Count - 1);

        PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(new EntryCollection { entries = entries }));
        PlayerPrefs.Save();
    }
}
