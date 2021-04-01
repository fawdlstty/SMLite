package cn.fawdlstty.smlite.internface;

public interface IAction_t<TTrigger extends Enum> {
    public void call (TTrigger trigger) throws Exception;
}
