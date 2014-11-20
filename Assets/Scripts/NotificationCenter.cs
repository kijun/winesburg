using System;
using System.Collections.Generic;

public delegate void NotificationHandler();

public class NotificationCenter {

    // class properties / methods

	private static NotificationCenter defaultCenterInstance;

	public static NotificationCenter defaultCenter() {
        if (instance == null) defaultCenterInstance = new NotificationCenter();
        return defaultCenterInstance;
	};

    // object properties

    private Dictionary<string, HashSet<NotificationHandler>> keyToHandlerMap = new Dictionary<string, HashSet<NotificationHandler>>();

    // object methods

	private NotificationCenter () {}

    public void AddObserver(string notificationName, NotificationHandler handler) {
        if (!keyToHandlerMap.ContainsKey(notificationName)) {
            keyToHandlerMap.Add(notificationName, new HashSet<NotificationHandler>());
        }
        keyToHandlerMap[notificationName].Add(handler);
	}

    public void RemoveObserversWithName(string notificationName) {
        keyToHandlerMap.Remove(notificationName);
    }

    public void RemoveObserver(string notificationName, NotificationHandler handler) {
        if (keyToHandlerMap.ContainsKey(notificationName)) {
            var handlers = keyToHandlerMap[notificationName];
            handlers.Remove(handler);
            if (handlers.Count == 0) {
                keyToHandlerMap.Remove(notificationName);
            }
        }
    }

    public void PostNotification(string notificationName) {
        foreach (NotificationHandler handler in keyToHandlerMap[notificationName]) {
            handler();
        }
    }
}
