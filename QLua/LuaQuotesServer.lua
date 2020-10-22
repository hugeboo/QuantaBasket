-- Сервер котировок L1

stopped = false -- Остановка файла
socket = require("socket") -- Указатель для работы с sockets
json = require( "json" ) -- Указатель для работы с json
IPAddr = "127.0.0.1" --IP Адрес
IPPort = 3585		 --IP Port	 
client = nil
quoteFilter = {}

function OnInit(path)
    server = assert(socket.bind("*", IPPort))
    message(string.format("Quotes Server started. IP: %s; Port: %d\n", IPAddr, IPPort), 1);
end;

function OnStop(signal)
    if client then
        client:send("{}")
        sleep(1000) 
        client:close() 
    end
    stopped = true; -- Остановили исполнение кода 
end;
  
  -- Функция вызывается перед закрытием квика
function OnClose()
    if client then 
        client:send("{}") 
        sleep(1000) 
        client:close() 
    end
    stopped = true; -- закрыли квик, надо остановить исполнение кода
end;

function OnParam(class_code, sec_code)
    if (client and CheckByQuoteFilter(class_code, sec_code) == 1) then
        tlast = getParamEx(class_code, sec_code, "last").param_value
        task = getParamEx(class_code, sec_code, "offer").param_value
        tbid = getParamEx(class_code, sec_code, "bid").param_value
        tvoltoday = getParamEx(class_code, sec_code, "voltoday").param_value -- проторгованный объем в штуках

        ttime = getInfoParam("SERVERTIME")

        -- сразу кидаем подключенному клиенту (если такого нет - улетает в воздух)
        client:send(json.encode({class = class_code, sec = sec_code, 
            last = tlast, bid = tbid, ask = task, voltoday = tvoltoday, time = ttime}))
    end
end

-- проверка на критерий подписки 
function CheckByQuoteFilter(classCode, secCode)
    for i = 1, #quoteFilter, 1 do
      local fc = quoteFilter[i].c
      local fs = quoteFilter[i].s
      if (((fc == "*") or (fc == classCode)) and 
          ((fs == "*") or (fs == secCode))) then return 1 end
    end
    return 0
end;

function main()
    -- wait for a connection from any client
      while not stopped do
        client = server:accept()
        --sleep(1)
        -- причепился новый клиент, читаем от него фильтр подписки
        local line, err = client:receive()
        quoteFilter = json.decode(line);
      end
end;