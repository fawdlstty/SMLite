package cn.fawdlstty.smlite.internface;

public interface IAction_t<TTrigger> {
    public void call (TTrigger trigger) throws Exception;
}
