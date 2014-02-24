OnionLog
========

OnionLogger is a stack-based logging tool. Most logging utilities generate a monolithic block of logging statements. OnionLogger allows you to organize your logging statements hierarchally by pushing and popping layers on a stack. Each time you push a layer, your log is indented so that it is easy to see which logging statements are enclosed in which layer. In addition, whenever you pop a layer, the total time spent in the layer will also be logged.

This tool is licensed under the MIT License, available here: http://opensource.org/licenses/MIT

It is very barebones right now, and does not perform any error checking. Use at your own risk! (until I make it better!)

Here is some sample code for how to set up and use an OnionLogger:

    void Start () {
      // Individual OnionLoggers can be instantiated, or the singleton "globalLog" can
      // be used, as long as it is instantiated first.
      OnionLogger.globalLog = new OnionLogger();
      OnionLogger.globalLog.LoggingLevel = OnionLogger.LoggingLevels.TRACE;
      LogTest();
    }
  
    void LogTest () {
      // OnionLogger.globalLog is accessible from any class.
      // I'm assigning it to the variable "log" for readability, but logging can
      // be called directly on the globalLog if desired.
      OnionLogger log = OnionLogger.globalLog;
      
      // Indentations in code are for clarification only; not required.
    
      log.PushInfoLayer("TestLog");
        log.LogInfo("This is an INFO level log message.");
        log.PushInfoLayer("Procedure");
          log.LogWarn("An error might be coming up...");
          log.LogTrace("x = 5 / 0");
          log.LogError("Divided by zero!");
          log.PushErrorLayer("ErrorHandling");
            logError("Handled error successfully.");
          log.PopErrorLayer();
          log.PushDebugLayer("AnotherProcedure");
            // layers can be empty
          log.PopDebugLayer();
        log.PopInfoLayer();
      log.PopInfoLayer();
    }
  
This generates the following log:

    INFO  TestLog started at 2/24/2014 2:28:05 AM
    INFO    This is an INFO level log message.
    INFO    Procedure started at 2/24/2014 2:28:05 AM
    WARN      An error might be coming up...
    TRACE     x = 5 / 0
    ERROR     Divided by zero!
    ERROR     ErrorHandling started at 2/24/2014 2:28:05 AM
    ERROR       Handled error successfully.
    ERROR     ErrorHandling ended after 0.5214ms
    DEBUG     AnotherProcedure started at 2/24/2014 2:28:05 AM
    DEBUG     AnotherProcedure ended after 0.1223ms
    INFO    Procedure ended after 1.3961ms
    INFO  TestLog ended after 10.424ms
