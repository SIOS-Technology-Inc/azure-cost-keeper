FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /opt/ack
COPY bin/ /opt/ack/bin/

RUN chmod 700 /opt/ack/bin/CostKeeper.Program.CostAlertBatch

CMD ["/opt/ack/bin/CostKeeper.Program.CostAlertBatch"]
