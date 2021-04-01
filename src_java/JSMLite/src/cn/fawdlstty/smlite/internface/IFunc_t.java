package cn.fawdlstty.smlite.internface;

public interface IFunc_t<TState extends Enum, TTrigger extends Enum> {
    public TState call (TTrigger trigger) throws Exception;
}
